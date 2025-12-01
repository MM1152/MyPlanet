using UnityEngine;

public class Bullet : ProjectTile
{
    private float durationTime = 3f;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Bullet;
        durationTime = 3f;
    }

    protected override void Update()
    {
        durationTime -= Time.deltaTime;
        if(durationTime <= 0f)
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