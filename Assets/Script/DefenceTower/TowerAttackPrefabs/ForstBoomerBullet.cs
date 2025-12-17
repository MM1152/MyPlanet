using UnityEngine;

public class ForstBoomerBullet : MagmaBoomFregment
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.FrostBoomerBullet;
    }
}
