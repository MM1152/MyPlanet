using UnityEngine;
using System;

public class FragmentBullet : ProjectTile
{
    private float currentDuration = 0f;

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.FragmentBullet;
        currentDuration = tower.BonusFregmentRange / speed;
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
