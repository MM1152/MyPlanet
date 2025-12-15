using UnityEngine;

public class BlackMineBullet : Bullet
{
    public float gravityScale;
    private float attackTimer;
    private float attackInterval = 0.1f;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.BlackMineBullet;
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

    protected override void Update()
    {
        base.Update();

        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
        }

        attackTimer += Time.deltaTime;
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
                    var enemyType = collision.GetComponent<Enemy>().enemyType;

                    var percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    find.OnDamage((int)(tower.CalcurateAttackDamage * percent));

                    if (enemyType == EnemyType.EliteMonster || enemyType == EnemyType.Boss)
                        return;

                    var dir = (transform.position - collision.transform.position).normalized;
                    collision.transform.position += dir * gravityScale * Time.deltaTime;

                }
               
            }
        }
    }
}