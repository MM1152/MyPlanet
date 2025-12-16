using Unity.VisualScripting;
using UnityEngine;

public class Bullet : ProjectTile
{
    private PoolsId particleId;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Bullet;
    }

    protected override void Update()
    {
        Move();
    }
    
    public void SetHitParticle(PoolsId particleId)
    {
        this.particleId = particleId;
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