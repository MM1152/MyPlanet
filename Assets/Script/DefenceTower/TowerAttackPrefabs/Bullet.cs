using UnityEngine;

public class Bullet : ProjectTile
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Bullet;
    }

    protected override void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0f)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            return;
        }
        Move();
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);        
    }
}