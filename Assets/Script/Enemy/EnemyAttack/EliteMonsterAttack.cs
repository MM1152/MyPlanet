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
        { ElementType.Ice, new SpreadShot() },
        { ElementType.Steel, new TrailShotAttack() },
        { ElementType.Light, new RotatingLaserAttack() },
        { ElementType.Dark, new NormalStrategy() },
    };
    }
}
