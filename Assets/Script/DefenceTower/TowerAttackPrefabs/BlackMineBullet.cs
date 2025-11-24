using UnityEngine;

public class BlackMineBullet : Bullet
{
    public float gravityScale;
    private float duration;
    private float attackTimer;
    private float attackInterval = 0.1f;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.BlackMineBullet;
        duration = tower.BonusAttackRange / FullBulletSpeed;
        attackTimer = 0;
        transform.localScale = new Vector3(tower.BonusExplosionRange, tower.BonusExplosionRange);
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }

    protected override Vector3 SetDir()
    {
        return base.SetDir();
    }

    private void FixedUpdate()
    {
        if(attackTimer >= attackInterval)
        {
            attackTimer = 0f;
        }

        attackTimer += Time.deltaTime;
    }

    protected override void Update()
    {
        base.Update();

        duration -= Time.deltaTime;
        if(duration <= 0f)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag(TagIds.EnemyTag))
        {
            if(attackTimer >= attackInterval)
            {
                var find = collision.GetComponent<IDamageAble>();
                if(find != null)
                {
                    var dir = (transform.position - collision.transform.position).normalized;
                    collision.transform.position += dir * gravityScale * Time.deltaTime;

                    var percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    find.OnDamage((int)(tower.FullDamage * percent));
                }
               
            }
        }
    }
}