using UnityEngine;

public class SpreadBullet : EnemyProjectileSimple
{
    private Vector3 movedir;
    private Vector3 originPosition;
    private float dis;

    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);
        poolsId = PoolsId.SpreadBullet;
    }

    public void SetDirection(Vector3 direction, float distance)
    {
        movedir = direction.normalized;
        dis = distance;
        originPosition = transform.position;
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
        if (Vector2.Distance(transform.position, originPosition) < dis)
        {
            transform.position += movedir * currentSpeed * Time.deltaTime;
        }
        else
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }
}
