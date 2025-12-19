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
        if(gameObject.activeSelf)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);

            if( poolsId != PoolsId.None)
            { 
                var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
                particle.transform.position = collision.ClosestPoint(transform.position);
                poolsId = PoolsId.None;
            }   
        }
    }
}
