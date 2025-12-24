using UnityEngine;

public class LuminaSniperBullet : Bullet
{
    private int homingCount = 0;
    private LineRenderer lineRenderer;
    public new float duration = 0.2f;
    private float timer = 0f;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.LuminaSniperBullet;
        homingCount = tower.BonusTargetingCount;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, data.TowerGameObject.transform.position);
        timer = 0f;
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    protected override void Update()
    {
        timer += Time.deltaTime;
        if (target == null && targetDamageAble.IsDead || homingCount <= 0)
        {
            if (timer > duration)
            {
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            }
            return;
        }

        var enemys = tower.towerManager.FindTargets(target.transform.position);

        if (enemys == null || enemys.Count == 0) return;
        enemys.RemoveAt(0);
        while (homingCount > 0)
        {
            homingCount--;
            lineRenderer.positionCount++;

            if (target != null && !targetDamageAble.IsDead)
            {
                var barrier = target.GetComponentInChildren<Barrier>();
                if (barrier != null && !barrier.IsDead)
                {
                    var barrierDamageAble = barrier.GetComponent<IDamageAble>();
                    if (barrierDamageAble != null)
                    {
                        var col = barrier.Collider;
                        var startPos = lineRenderer.GetPosition(lineRenderer.positionCount - 2);
                        float percent = tower.TypeEffectiveness.GetDamagePercent(barrier.ElementType);
                        barrierDamageAble.OnDamage((int)(tower.CalcurateAttackDamage * percent));
                        var endPos = col.ClosestPoint(startPos);
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, endPos);
                        continue;
                    }
                }

                var find = target.GetComponent<IDamageAble>();
                if (find != null)
                {
                    float percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, target.transform.position);
                }

                if (enemys.Count != 0)
                {
                    SetTarget(enemys[0].transform, noise);
                    enemys.RemoveAt(0);
                }
            }
            else
            {
                lineRenderer.positionCount--;
                homingCount = 0;
                break;
            }

        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        if (target != collision.gameObject.transform)
        {
            return;
        }


        if (homingCount > 0)
        {
            homingCount--;
            if (enemy != null)
            {

            }
            else
            {
                if (gameObject.activeSelf)
                    Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            }
        }
        else
        {
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

}
