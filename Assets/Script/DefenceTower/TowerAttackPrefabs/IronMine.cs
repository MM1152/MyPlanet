using CsvHelper;
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
        if (gameObject.activeSelf)
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);

        var explosion = CreateExplosion();
        explosion.Init(tower);
        explosion.transform.position = this.transform.position;

        //var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Hit17novaviolet);
        //hitParticle.transform.position = this.transform.position;
    }

    protected override Explosion CreateExplosion()
    {
        var hit = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Hit17novaviolet);
        hit.transform.position = this.transform.position;
        return Managers.ObjectPoolManager.SpawnObject<IronMineExplosion>(PoolsId.IronMineExplosion);
    }
}