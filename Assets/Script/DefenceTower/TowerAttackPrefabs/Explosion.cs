using UnityEngine;

public class Explosion : BaseAttackPrefab
{
    private float duration = 0.1f;
    private float timer = 0f;
    private float explosionSize;
    public override void Init(Tower data)
    {
        base.Init(data);

        timer = 0f;
        duration = 0.3f;
        explosionSize = data.BonusExplosionRange;

        transform.localScale = Vector3.zero;
        poolsId = PoolsId.Explosion;
    }

    protected override void HitTarget(Collider2D collision)
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
            float percent = typeEffectiveness.GetDamagePercent(barrier.ElementType);
            barrier.OnDamage((int)(tower.CalcurateAttackDamage * percent));
            return;
        }

        var find = enemy.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
            find.StatusEffect?.Apply(effect, find);
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;

        var scaleUpSpeed = explosionSize / duration;
        this.transform.localScale += new Vector3(scaleUpSpeed, scaleUpSpeed, 0) * Time.deltaTime;

        if (timer >= duration)
        {
            if(gameObject.activeSelf)
            {
               Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            }
        }
    }
}