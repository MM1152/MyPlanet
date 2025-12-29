using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class VortexLaserAttack : IShotStrategy
{
    private Enemy enemy;
    private bool initialized = false;
    private bool isAttacking = false;
    private Rect screenRect;
    private List<float> damageIntervals = new List<float>();
    private float alpha = 1f;
    private float rotationSpeed = 60f;
    private float rotateTime = 3f;
    private float rotateDelay = 2f;
    private int lineCount = 4;
    private float angle = 360f;
    private LineRenderer baseLineRenderer;
    private List<LineRenderer> lineRenderers;
    private List<float> currentAngles = new List<float>();
    private LayerMask obstacleMask;
    private Vector2 startPoint = Vector2.zero;
    private List<ParticleSystem> hitParticle;
    private List<ParticleSystem> flashParticle;

    public void Shot(Enemy enemy, GameObject target)
    {
        if (!initialized) Initialize(enemy);
        if (isAttacking) return;

        LaserRotation(enemy.GetCancellationTokenOnDestroy()).Forget();
    }

    private void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
        initialized = true;
        screenRect = Utils.GetScreenRect();   
        baseLineRenderer = enemy.enemyLineRenderer;
        lineRenderers = new List<LineRenderer>();
        baseLineRenderer.startWidth = enemy.transform.localScale.y * 0.2f;
        baseLineRenderer.endWidth = enemy.transform.localScale.y * 0.2f;
        baseLineRenderer.enabled = true;
        baseLineRenderer.positionCount = 2;
        lineRenderers.Add(baseLineRenderer);
        currentAngles.Add(0f);
        damageIntervals.Add(0f);
        flashParticle = new List<ParticleSystem>();
        hitParticle = new List<ParticleSystem>();
        for(int i =0 ; i<= lineCount; i++)
        {
            flashParticle.Add(null);
            hitParticle.Add(null);
        }

        for (int i = 0; i < lineCount; i++)
        {
            GameObject laserObj = new GameObject();
            laserObj.transform.SetParent(enemy.transform);
            laserObj.transform.localPosition = Vector3.zero;

            var lr = laserObj.AddComponent<LineRenderer>();
            lr.enabled = true;
            lr.positionCount = 2;
            lr.startWidth = baseLineRenderer.startWidth;
            lr.endWidth = baseLineRenderer.endWidth;
            if (baseLineRenderer.material != null)
            {
                lr.material = baseLineRenderer.material;
            }
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            lineRenderers.Add(lr);
            currentAngles.Add((i + 1) * (angle / (lineCount + 1)));
            damageIntervals.Add(0f);
        }
        enemy.OnDie += ClearLineRenderers;
    }

    private async UniTask LaserRotation(System.Threading.CancellationToken cancellationToken)
    {
        isAttacking = true;

        try
        {
            float elapsedTime = 0f;

            while (elapsedTime < rotateTime)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                UpdateLaser(true);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield(cancellationToken);
            }

            float delayTime = 0f;
            while (delayTime < rotateDelay)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                UpdateLaser(false);
                delayTime += Time.deltaTime;
                await UniTask.Yield(cancellationToken);
            }

        }
        catch (System.OperationCanceledException)
        {
            // Enemy가 파괴됨
        }
        finally
        {
            isAttacking = false;
        }
    }
    private void UpdateLaser(bool rotate)
    {
        if (enemy == null || lineRenderers == null) return;
        
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            if (enemy == null) return;

            damageIntervals[i] -= Time.deltaTime;

            if (rotate)
            {
                currentAngles[i] += rotationSpeed * Time.deltaTime;
            }

            float lineAngle = currentAngles[i];
            Vector2 dir = new Vector2(Mathf.Cos(lineAngle * Mathf.Deg2Rad), Mathf.Sin(lineAngle * Mathf.Deg2Rad)).normalized;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, dir, Mathf.Max(screenRect.width + alpha, screenRect.height + alpha), obstacleMask);
            Vector2 endPoint = hit.collider != null ? hit.point : ((Vector2)enemy.transform.position + dir * Mathf.Max(screenRect.width + alpha, screenRect.height + alpha));
            startPoint = enemy.transform.position + (Vector3)dir * (enemy.transform.localScale.x * 0.5f);
            lineRenderers[i].SetPosition(0, startPoint);
            FlashParticle(startPoint, dir, Vector2.Distance(startPoint, endPoint), i);

            if (hit.collider != null && damageIntervals[i] <= 0f)
            {
                lineRenderers[i].SetPosition(1, endPoint);
                HitParticle(endPoint, i);
                var find = hit.collider.GetComponent<IDamageAble>();
                if (find != null)
                {
                    float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    find.OnDamage(Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue));
                    damageIntervals[i] = 0.5f;
                }
            }
            else
            {
                lineRenderers[i].SetPosition(1, endPoint);
                HitParticle(endPoint, i);
            }
        }
    }

    private void ClearLineRenderers(Enemy enemy)
    {
        if (lineRenderers == null) return;

        foreach (var lr in lineRenderers)
        {
            if (lr != null && lr != baseLineRenderer)
            {
                Object.Destroy(lr.gameObject);
            }
        }
        lineRenderers = null;
        initialized = false;
        for (int i = 0; i < hitParticle.Count; i++)
        {
            if (hitParticle[i] != null)
            {
                Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedHit, hitParticle[i].gameObject);
            }
        }
        for (int i = 0; i < flashParticle.Count; i++)
        {
            if (flashParticle[i] != null)
            {
                Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedFlash, flashParticle[i].gameObject);
            }
        }
        hitParticle.Clear();
        flashParticle.Clear();
    }

    private void FlashParticle(Vector2 position, Vector2 direction, float dis, int index)
    {
        if (flashParticle[index] == null)
        {
            flashParticle[index] =Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedFlash);

            if (flashParticle[index] == null) return;
            flashParticle[index].Play();
        }

        if (flashParticle[index].transform.position == (Vector3)position) return;
        flashParticle[index].transform.position = position;
        flashParticle[index].transform.rotation = Quaternion.LookRotation(direction);

        var flashmain = flashParticle[index].main;
        flashmain.startRotation = dis / flashmain.startSpeed.constant;
    }

    private void HitParticle(Vector2 position, int index)
    {
        if (hitParticle[index] != null)
        {
            if (hitParticle[index].transform.position == (Vector3)position)
                return;
            hitParticle[index].transform.position = position;
            return;
        }
        hitParticle[index] =Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedHit);
        if (hitParticle[index] == null) return;
        hitParticle[index].transform.position = position;
        hitParticle[index].Play();
    }
}
