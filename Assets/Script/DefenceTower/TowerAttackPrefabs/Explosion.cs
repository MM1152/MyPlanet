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
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
            find.StatusEffect.Apply(effect, find);
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;

        var scaleUpSpeed = explosionSize / duration;
        this.transform.localScale += new Vector3(scaleUpSpeed, scaleUpSpeed, 0) * Time.deltaTime;

        if (timer >= duration)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }
}