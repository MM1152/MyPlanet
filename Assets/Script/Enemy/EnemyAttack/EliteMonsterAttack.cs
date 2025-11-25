using UnityEngine;
using System.Collections.Generic;

public class EliteMonsterAttack : BaseShotAttack
{
    public EliteMonsterAttack()
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
