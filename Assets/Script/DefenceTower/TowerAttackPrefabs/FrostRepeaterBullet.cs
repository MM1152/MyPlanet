using UnityEngine;

public class FrostRepeaterBullet : Bullet
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.FrostRepeaterBullet;
    }
}
