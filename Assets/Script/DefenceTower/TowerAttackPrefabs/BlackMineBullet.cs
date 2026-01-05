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
                var enemy = collision.GetComponent<Enemy>();
                if (enemy == null && collision.attachedRigidbody != null)
                {
                    enemy = collision.attachedRigidbody.GetComponentInParent<Enemy>();
                }
                if (enemy == null) return;

                var barrier = enemy.GetComponentInChildren<Barrier>();
                if (barrier != null && !barrier.IsDead)
                {
                    var percent = tower.TypeEffectiveness.GetDamagePercent(barrier.ElementType);
                    barrier.OnDamage((int)(tower.CalcurateAttackDamage * percent));
                    Managers.SoundManager.PlaySFX(hitSoundId);
                    return;
                }

                var find = enemy.GetComponent<IDamageAble>();
                if(find != null)
                {
                    var percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
                    Managers.SoundManager.PlaySFX(hitSoundId);

                    if (enemy.enemyType == EnemyType.EliteMonster || enemy.enemyType == EnemyType.Boss)
                        return;

                    var dir = (transform.position - collision.transform.position).normalized;
                    collision.transform.position += dir * gravityScale * Time.deltaTime;
                }
            }
        }
    }
}