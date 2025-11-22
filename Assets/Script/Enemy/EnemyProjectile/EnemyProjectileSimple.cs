using UnityEngine;

public class EnemyProjectileSimple : EnemyProjectileBase
{

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(enemyData.atk * percent));
        }
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
        transform.position += dir * enemyData.bulletSpeed * Time.deltaTime;
    }

    protected override void BlockedHit(Collider2D collision)
    {
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
    }
}
