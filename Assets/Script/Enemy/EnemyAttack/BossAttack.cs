using UnityEngine;

public class BossAttack : BaseShotAttack
{
    public BossAttack()
    {
        shotStrategies = new System.Collections.Generic.Dictionary<ElementType, IShotStrategy>()
        {
            { ElementType.Normal, new NormalStrategy() },
            { ElementType.Fire, new NormalStrategy() },
            { ElementType.Ice, new NormalStrategy() },
            { ElementType.Steel, new RapidFireAttack() },
            { ElementType.Light, new NormalStrategy() },
            { ElementType.Dark, new NormalStrategy() },
        };
    }
}
