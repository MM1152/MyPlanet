using UnityEngine;

public class FragmentBullet : ProjectTile
{
    private float duration = 1f;
    private float currentDuration = 0f;

    public override void Init(Tower data, TypeEffectiveness typeEffectiveness, IStatusEffect effect)
    {
        base.Init(data, typeEffectiveness, effect);
        poolsId = PoolsId.FragmentBullet;
        currentDuration = duration;
    }

    protected override void Update()
    {
        Move();
        currentDuration -= Time.deltaTime;

        if (currentDuration <= 0f)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
    }
}
