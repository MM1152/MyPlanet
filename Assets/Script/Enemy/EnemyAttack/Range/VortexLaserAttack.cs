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
    private LayerMask obstacleMask = LayerMask.GetMask("DefenseTower", "Player");

    public void Shot(Enemy enemy, GameObject target)
    {
        if (!initialized) Initialize(enemy);
        if (isAttacking) return;

        LaserRotation(enemy.GetCancellationTokenOnDestroy()).Forget();
    }

    private void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        initialized = true;
        if (enemy.WaveManager == null)
        {
            var camera = Camera.main;

            if (camera == null) return;

            var zDistance = Mathf.Abs(camera.transform.position.z);

            var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
            var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

            screenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }
        else
        {
            screenRect = enemy.WaveManager.ScreenBounds;
        }
        baseLineRenderer = enemy.enemyLineRenderer;
        lineRenderers = new List<LineRenderer>();
        baseLineRenderer.startWidth = enemy.transform.localScale.y * 0.2f;
        baseLineRenderer.endWidth = enemy.transform.localScale.y * 0.2f;
        baseLineRenderer.enabled = true;
        baseLineRenderer.positionCount = 2;
        lineRenderers.Add(baseLineRenderer);
        currentAngles.Add(0f);
        damageIntervals.Add(0f);

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

            lineRenderers[i].SetPosition(0, enemy.transform.position);
            lineRenderers[i].SetPosition(1, endPoint);

            if (hit.collider != null && damageIntervals[i] <= 0f)
            {
                var find = hit.collider.GetComponent<IDamageAble>();
                if (find != null)
                {
                    float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    find.OnDamage(Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue));
                    damageIntervals[i] = 0.5f;
                }
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
    }
}
