using UnityEngine;

public class Bullet : ProjectTile
{
    public override void Init(Tower data, TypeEffectiveness typeEffectiveness, IStatusEffect effect)
    {
        base.Init(data, typeEffectiveness, effect);
        poolsId = PoolsId.Bullet;
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);        
    }
}