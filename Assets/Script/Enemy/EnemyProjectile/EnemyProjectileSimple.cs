using UnityEngine;

public class EnemyProjectileSimple : EnemyProjectileBase
{
    protected PoolsId particleId;
    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage(Mathf.Clamp((int)((Enemy.atk - find.Defense) * percent), 1, int.MaxValue));
        }
    }

    public void SetHitParticle(PoolsId particleId)
    {
        this.particleId = particleId;
    }

    private void Update()
    {
        if (target == null || targetDamageAble.IsDead)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            return;
        }

        Move();
    }

    protected virtual void Move()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * currentSpeed * Time.deltaTime;
    }

    protected override void BlockedHit(Collider2D collision)
    {
        if (gameObject.activeSelf)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);

            if (poolsId != PoolsId.None)
            { // 터졌을때 파티클 생성
                var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
                particle.transform.position = collision.ClosestPoint(transform.position);
                poolsId = PoolsId.None;
            }
        }
    }
}
