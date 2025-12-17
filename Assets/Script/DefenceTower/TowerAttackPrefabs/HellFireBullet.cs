using UnityEngine;

public class HellFireBullet : Bullet
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.HellFireBullet;
    }
}
