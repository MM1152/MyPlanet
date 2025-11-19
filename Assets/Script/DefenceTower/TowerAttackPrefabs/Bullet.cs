using UnityEngine;

public class Bullet : ProjectTile
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Bullet;
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);        
    }
}