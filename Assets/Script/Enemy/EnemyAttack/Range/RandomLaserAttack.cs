using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomLaserAttack : IShotStrategy
{
    private LineRenderer laserRenderer;
    private RaycastHit2D hit;
    private LayerMask obstacleMask;
    private bool isInitialized = false;
    private List<Vector2> laserPoints = new List<Vector2>();
    private Vector2 currentStartPoint;
    private float duration = 3; //설정테이블or 공속
    private float growTime = 0f;
    private Vector2 startPoint = Vector2.zero;
    private ParticleSystem hitParticle;
    private ParticleSystem flashParticle;
    private float startWidth = 0f;
    private float endWidth = 0f;
    private float lineWidth = 0f;
    private List<GameObject> bossPartnerObject = new List<GameObject>();
    private float partnerScale = 0.05f;
    private int maxPartnerCount = 2;
    private int[] partnerPositionsX = new int[] { 0, 2 };
    private Rect screenRect;
    private float alpha = 1f;
    public void Init(Enemy enemy)
    {
        screenRect = Utils.GetScreenToWorldRect();
        laserPoints.Clear();
        var centerY = (enemy.target.transform.position.y + screenRect.yMax) / 2;
        laserPoints.Add(new Vector2(screenRect.xMin + (enemy.enemyCollider.radius * 2f + alpha), centerY)); // Left
        laserPoints.Add(new Vector2((screenRect.xMin + screenRect.xMax) / 2, centerY)); // Mid
        laserPoints.Add(new Vector2(screenRect.xMax - (enemy.enemyCollider.radius * 2f + alpha), centerY)); // Right

        for (int i = 0; i < maxPartnerCount; i++)
        {
            var partner = Managers.ObjectPoolManager.SpawnObject<Transform>(PoolsId.BossLaserPartnerPos);
            partner.gameObject.SetActive(true);
            partner.localScale = Vector3.one * partnerScale;
            partner.position = new Vector3(laserPoints[partnerPositionsX[i]].x, laserPoints[partnerPositionsX[i]].y, 0f);
            bossPartnerObject.Add(partner.gameObject);
        }
    }
    private void InitializeLaserRenderer(Enemy enemy)
    {
        if (isInitialized == false)
        {
            obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
            laserRenderer = enemy.enemyLineRenderer;
            laserRenderer.enabled = true;
            laserRenderer.startWidth = enemy.transform.localScale.y * 0.4f;
            laserRenderer.endWidth = enemy.transform.localScale.y * 0.4f;
            startWidth = enemy.transform.localScale.y * 0.4f;
            endWidth = 0f;
            laserRenderer.positionCount = 2;
            isInitialized = true;
            growTime = 0f;
            currentStartPoint = laserPoints[Random.Range(0, laserPoints.Count)];
            Debug.Log($"{nameof(RandomLaserAttack)} 초기화 완료. 시작 지점: {currentStartPoint}");
            enemy.OnDie += LaserReset;
        }
    }

    private void ResetLaserPoint()
    {
        if (laserPoints.Count == 0) return;

        var index = Random.Range(0, laserPoints.Count);
        if (laserPoints[index] == currentStartPoint)
        {
            index = (index + 1) % laserPoints.Count;
        }
        currentStartPoint = laserPoints[index];
        laserRenderer.enabled = false;
        laserRenderer.positionCount = 0;
        growTime = 0f;
    }
    private void RotateTowardsTarget(Enemy enemy, Transform partner)
    {
        if (enemy.target == null) return;

        var dir = (enemy.target.transform.position - partner.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        partner.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void LaserUpdate(Enemy enemy, GameObject target)
    {
        if (laserRenderer == null) InitializeLaserRenderer(enemy);

        if (target == null)
        {
            if (laserRenderer != null) laserRenderer.enabled = false;
            return;
        }

        foreach (var bossPartner in bossPartnerObject)
        {
            RotateTowardsTarget(enemy, bossPartner.transform);
        }

        if (laserRenderer.startWidth <= 0f)
        {
            ResetLaserPoint();
            laserRenderer.enabled = true;
            laserRenderer.positionCount = 2;
            startWidth = enemy.transform.localScale.y * 0.4f;
            endWidth = 0f;
            laserRenderer.startWidth = startWidth;
            laserRenderer.endWidth = startWidth;
        }

        Vector2 dir = (target.transform.position - (Vector3)currentStartPoint).normalized;
        float dis = Vector2.Distance(currentStartPoint, target.transform.position);
        startPoint = currentStartPoint + dir * (enemy.transform.localScale.x * 0.5f);
        FlashParticle(startPoint, dir, dis);
        laserRenderer.SetPosition(0, startPoint);
        hit = Physics2D.Raycast(startPoint, dir, dis, obstacleMask);
        if (hit.collider != null)
        {
            laserRenderer.SetPosition(1, hit.point);
            HitParticle(hit.point);
        }

        growTime += Time.deltaTime;
        float t = growTime / duration;
        lineWidth = Mathf.Lerp(startWidth, endWidth, t);
        laserRenderer.startWidth = lineWidth;
        laserRenderer.endWidth = lineWidth;
    }

    public void Shot(Enemy enemy, GameObject target)
    {
        if (target == null)
        {
            if (laserRenderer != null) laserRenderer.enabled = false;
            return;
        }

        if(hit.collider == null) return;    

        var find = hit.collider.GetComponent<IDamageAble>();
        if (find != null)
        {
            Debug.Log($"[RandomLaserAttack] 적중 대상: {find}, 위치: {hit.point}");
            float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
            var damage = Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue);
            find.OnDamage(damage);
            enemy.OnHeal(damage / 2);
        }
    }

    private void FlashParticle(Vector2 position, Vector2 direction, float dis)
    {
        if (flashParticle == null)
        {
            flashParticle = Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedFlash);

            if (flashParticle == null) return;
            flashParticle.Play();
        }

        if (flashParticle.transform.position == (Vector3)position) return;

        flashParticle.transform.position = position;
        flashParticle.transform.rotation = Quaternion.LookRotation(direction);

        var flashmain = flashParticle.main;
        flashmain.startRotation = dis / flashmain.startSpeed.constant;
    }

    private void HitParticle(Vector2 position)
    {
        if (hitParticle != null)
        {
            if (hitParticle.transform.position == (Vector3)position)
                return;
            hitParticle.transform.position = position;
            return;
        }

        hitParticle = Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedHit);
        if (hitParticle == null) return;
        hitParticle.transform.position = position;
        hitParticle.Play();
    }

    public void LaserReset(Enemy enemy)
    {
        if (laserRenderer != null)
        {
            laserRenderer.enabled = false;
            laserRenderer.positionCount = 0;
            Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedHit, hitParticle?.gameObject);
            Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedFlash, flashParticle?.gameObject);
            hitParticle = null;
            flashParticle = null;
        }
        foreach (var bossPartnerObject in bossPartnerObject)
        {
            bossPartnerObject.transform.SetParent(null);
            Managers.ObjectPoolManager.Despawn(PoolsId.BossLaserPartner, bossPartnerObject);
        }
        bossPartnerObject.Clear();
        isInitialized = false;
        enemy.OnDie -= LaserReset;
    }
}
