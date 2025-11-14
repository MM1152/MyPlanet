using UnityEngine;

public class Bullet : ProjectTile
{
    public override void Init(Tower data, TypeEffectiveness typeEffectiveness, IStatusEffect effect)
    {
        base.Init(data, typeEffectiveness, effect);
        poolsId = PoolsId.Bullet;
    }

    protected override void HitTarget()
    {
        base.HitTarget();
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        //Destroy(gameObject);
    }
}