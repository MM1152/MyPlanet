using UnityEngine;

public class BurstBlasterBullet : Bullet
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.BurstBlasterBullet;
    }
}
