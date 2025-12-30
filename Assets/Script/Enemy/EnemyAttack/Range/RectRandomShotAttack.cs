using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RectRandomShotAttack : IShotStrategy
{
    private Rect screenBounds;
    private LayerMask targetLayer = LayerMask.GetMask("DefenseTower", "Player"); // 타겟 레이어
    private Vector3 startPoint;
    private bool isLaserActive = false;
    private bool initialized = false;
    private LineRenderer lineRenderer;

    private float minY => screenBounds.yMin;
    private float maxY => screenBounds.yMax;
    private float minX => screenBounds.xMin;
    private float maxX => screenBounds.xMax;

    private float laserDuration = 2f;
    private float laserTimer = 0f;
    private GameObject target;
    private Enemy enemy;

    private bool targetInRange = false;


    public void Shot(Enemy enemy, GameObject target)
    {
        if (isLaserActive) return;

        this.target = target;
        this.enemy = enemy;

        if (lineRenderer == null || !initialized)
        {
            lineRenderer = enemy.enemyLineRenderer;
            LineInitialized(enemy);
        }

        startPoint = Vector3.zero;
        int edge = Random.Range(0, 4);


        SetRandomPosition(edge, out startPoint);
        Debug.Log($"RectRandomShotAttack StartPoint: {startPoint}");
        LaserUpdate(enemy.GetCancellationTokenOnDestroy()).Forget();
    }

    private void ShotSet()
    {
        var bullet = CreateProjectile(PoolsId.SimpleBullet);
        bullet.transform.position = startPoint;
        bullet.Init(enemy, enemy.typeEffectiveness);

        if (target != null)
        {
            bullet.SetTarget(target.transform);
        }
        enemy.attackInterval = 0f;
    }

    private void LineInitialized(Enemy enemy)
    {
        screenBounds = Utils.GetScreenToWorldRect();
        lineRenderer.enabled = true;
        lineRenderer.startWidth = enemy.transform.localScale.x * 0.3f;
        lineRenderer.endWidth = enemy.transform.localScale.x * 0.3f;
        lineRenderer.positionCount = 2;
        initialized = true;
    }

    private void SetRandomPosition(int edge, out Vector3 position)
    {
        switch (edge)
        {
            case 0: // 위쪽
                position = new Vector3(Random.Range(minX, maxX), maxY, 0f);
                break;
            case 1: // 아래쪽
                position = new Vector3(Random.Range(minX, maxX), minY, 0f);
                break;
            case 2: // 왼쪽
                position = new Vector3(minX, Random.Range(minY, maxY), 0f);
                break;
            case 3: // 오른쪽
                position = new Vector3(maxX, Random.Range(minY, maxY), 0f);
                break;
            default:
                position = Vector3.zero;
                break;
        }
    }

    private SimpleBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<SimpleBullet>(poolsId);
        SimpleBullet projectile = projectileObj.GetComponent<SimpleBullet>();
        if (enemy.target != null)
        {
            projectile.SetHitParticle(PoolsId.Hit13redlaser);
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
            flash.transform.position = startPoint;
            projectile.transform.position = flash.transform.position;
        }

        return projectile;
    }

    private async UniTask LaserUpdate(System.Threading.CancellationToken cancellationToken)
    {
        isLaserActive = true;
        laserTimer = 0f;

        if (lineRenderer != null)
        {
            float initialWidth = enemy.transform.localScale.x * 0.3f;
            lineRenderer.startWidth = initialWidth;
            lineRenderer.endWidth = initialWidth;
            lineRenderer.enabled = true;
        }

        try
        {
            while (laserTimer < laserDuration)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                LaserDraw();
                laserTimer += Time.deltaTime;
                await UniTask.Yield(cancellationToken);
            }
        }
        catch (System.OperationCanceledException)
        {
            return;
        }
        finally
        {

            if (lineRenderer != null)
            {
                try
                {
                    lineRenderer.enabled = false;
                    lineRenderer.positionCount = 0;
                }
                catch { }
            }

            isLaserActive = false;
            targetInRange = false;
        }
    }

    private async UniTask LaserShrink(System.Threading.CancellationToken cancellationToken)
    {
        try
        {
            float initialWidth = enemy.transform.localScale.x * 0.3f;
            float shrinkSpeed = initialWidth / laserDuration;

            while (lineRenderer.startWidth > 0f)
            {
                if (cancellationToken.IsCancellationRequested || lineRenderer == null)
                {
                    break;
                }

                lineRenderer.startWidth -= shrinkSpeed * Time.deltaTime;
                lineRenderer.endWidth -= shrinkSpeed * Time.deltaTime;
                if (lineRenderer.startWidth < 0f)
                {
                    lineRenderer.startWidth = 0f;
                    lineRenderer.endWidth = 0f;
                    isLaserActive = false;
                    ShotSet();
                    break;
                }
                await UniTask.Yield(cancellationToken);
            }
        }
        catch (System.OperationCanceledException)
        {
            return;
        }
    }

    private void LaserDraw()
    {

        if (lineRenderer == null || target == null || target.transform == null) return;

        if (!lineRenderer.enabled)
            return;

        if (lineRenderer.positionCount < 2)
        {
            try { lineRenderer.positionCount = 2; }
            catch { return; }
        }

        try
        {
            lineRenderer.SetPosition(0, startPoint);

            Vector2 targetPos = target.transform.position;
            Vector2 dir = (targetPos - (Vector2)startPoint).normalized;
            float dis = Vector2.Distance(startPoint, targetPos);

            RaycastHit2D hit = Physics2D.Raycast(startPoint, dir, dis, targetLayer);
            if (hit.collider != null)
            {
                Vector2 offsetPoint = hit.point + dir * 0.1f;
                lineRenderer.SetPosition(1, offsetPoint);
                if (!targetInRange)
                {
                    targetInRange = true;
                    LaserShrink(enemy.GetCancellationTokenOnDestroy()).Forget();
                }
            }
            else
            {
                lineRenderer.SetPosition(1, targetPos);
            }
        }
        catch
        {
            return;
        }
    }
}
