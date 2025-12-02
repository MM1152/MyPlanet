using System.Collections.Generic;
using UnityEngine;

public class ShotAttack : BaseShotAttack
{
    public ShotAttack()
    {
        shotStrategies = new Dictionary<ElementType, IShotStrategy>()
    {
        { ElementType.Normal, new NormalStrategy() },
        { ElementType.Fire, new HomingShot() },
        { ElementType.Steel, new NormalStrategy() },
        { ElementType.Ice, new SpreadShot() },
        { ElementType.Light, new LaserShot() },
        { ElementType.Dark, new NormalStrategy() },
    };
    }
}