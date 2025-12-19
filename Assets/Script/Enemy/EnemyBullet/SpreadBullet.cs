using UnityEngine;

public class SpreadBullet : EnemyProjectileSimple
{
    private float range;
    private Vector3 movedir;

    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);
        poolsId = PoolsId.SpreadBullet;
        range = data.attackRange;
    }

    public void SetDirection(Vector3 direction)
    {
        movedir = direction.normalized;
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        if (gameObject.activeSelf)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);

            if (poolsId != PoolsId.None)
            {
                var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
                particle.transform.position = collision.ClosestPoint(transform.position);
                poolsId = PoolsId.None;
            }
        }
    }

    protected override void Move()
    {
        if (Vector2.Distance(transform.position, Enemy.transform.position) < range)
        {
            transform.position += movedir * currentSpeed * Time.deltaTime;
        }
        else
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            //  isDespawned = true;
        }
    }
}
