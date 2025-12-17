using UnityEngine;

public class ShadowBursterFragment : FragmentBullet
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.ShadowBursterFragment;
    }
}
