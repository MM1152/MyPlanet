using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomLaserAttack : IShotStrategy
{
    private LineRenderer laserRenderer;
    private RaycastHit2D hit;
    private LayerMask obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
    private bool isInitialized = false;
    private bool isTargetHit = false;
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

    private void InitializeLaserRenderer(Enemy enemy)
    {
        if (isInitialized == false)
        {
            laserRenderer = enemy.enemyLineRenderer;
            laserRenderer.enabled = true;
            laserRenderer.startWidth = enemy.transform.localScale.y * 0.4f;
            laserRenderer.endWidth = enemy.transform.localScale.y * 0.4f;
            startWidth = enemy.transform.localScale.y * 0.4f;
            endWidth = 0f;
            laserRenderer.positionCount = 2;
            isInitialized = true;
            growTime = 0f;
            laserPoints.Clear();
            laserPoints.Add(enemy.transform.position - (enemy.transform.localScale.x * 0.5f) * Vector3.right); // Left 
            laserPoints.Add(enemy.transform.position); // Mid
            laserPoints.Add(enemy.transform.position + (enemy.transform.localScale.x * 0.5f) * Vector3.right); // Right
            currentStartPoint = laserPoints[Random.Range(0, laserPoints.Count)];
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

    public void LaserUpdate(Enemy enemy, GameObject target)
    {
        if (laserRenderer == null) InitializeLaserRenderer(enemy);

        if (target == null)
        {
            if (laserRenderer != null) laserRenderer.enabled = false;
            return;
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
        if (target == null || hit.collider == null)
        {
            if (laserRenderer != null) laserRenderer.enabled = false;
            return;
        }

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == target.layer)
            {
                var find = hit.collider.GetComponent<IDamageAble>();
                if (find != null)
                {
                    float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    var damage = Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue);
                    find.OnDamage(damage);
                    enemy.OnHeal(damage / 2);
                }
            }
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
        isInitialized = false;
    }
}
