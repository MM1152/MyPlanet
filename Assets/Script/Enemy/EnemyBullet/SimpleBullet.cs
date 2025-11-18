using UnityEngine;

public class SimpleBullet : EnemyProjectileSimple
{
    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);
        poolsId = PoolsId.SimpleBullet;
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);        
    }
}
