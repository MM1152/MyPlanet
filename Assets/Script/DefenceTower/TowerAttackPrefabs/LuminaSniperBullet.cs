using UnityEngine;

public class LuminaSniperBullet : Bullet
{
    private int hominCount = 0;

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.LuminaSniperBullet;
        hominCount = tower.BonusTargetingCount;
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    protected override void Update()
    {
        base.Update();
        if(target != null && !targetDamageAble.IsDead)
            SetDir();
        else
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        if(target != collision.gameObject.transform)
        {
            return;
        }

        var find = collision.GetComponent<IDamageAble>();
        if (find != null) {
            float percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
        }

        if(hominCount > 0)
        {
            hominCount--;
            var enemy = tower.towerManager.FindTargets(transform.position);
            if(enemy != null && enemy.Count > 2)
            {
                var target = enemy[1];
                SetTarget(target.transform, noise);
            }
            else
            {
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            }
        }
        else
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

}
