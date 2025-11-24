using UnityEngine;

public class IronMine : Mine
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.IronMine;
    }

    public void ForcingBoom()
    {
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);

        var explosion = CreateExplosion();
        explosion.Init(tower);
        explosion.transform.position = this.transform.position;
    }

    protected override Explosion CreateExplosion()
    {
        return Managers.ObjectPoolManager.SpawnObject<IronMineExplosion>(PoolsId.IronMineExplosion);
    }
}