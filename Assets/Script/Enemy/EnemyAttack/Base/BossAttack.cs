using UnityEngine;
using System.Collections.Generic;

public class BossAttack : BaseShotAttack
{
    private Dictionary<(ElementType, int), IShotStrategy> bossShotStrategies;
    public BossAttack()
    {
        bossShotStrategies = new Dictionary<(ElementType, int), IShotStrategy>()
        {
            { (ElementType.Normal, 0), new NormalStrategy() },
            { (ElementType.Fire, 0), new NormalStrategy() },
            { (ElementType.Fire, 3037), new RandomLaserAttack() },
            { (ElementType.Ice, 0), new NormalStrategy() },
            { (ElementType.Steel, 3027), new RapidFireAttack() },
            { (ElementType.Steel, 3032), new MissileRainAttack() },
            { (ElementType.Steel, 3042), new HomingArcAttack() }, 
            { (ElementType.Light, 3047), new VortexLaserAttack() }, 
            { (ElementType.Dark, 3052), new ShadowSummonAttack() },
        };
    }
    public override void Attack(Enemy enemy)
    {
        enemy.attackInterval += Time.deltaTime;

        var strategy = GetShotStrategy(enemy.ElementType, enemy.enemyData.ID);
        
        
        if (strategy is RandomLaserAttack randomLaserAttack)
        {
            randomLaserAttack.LaserUpdate(enemy, enemy.GetTarget());
        }

        if (enemy.attackInterval >= enemy.fireInterval)
        {
            strategy.Shot(enemy, enemy.GetTarget());
            enemy.attackInterval = 0f;
            // enemy.ReturnMoveAction?.Invoke();
        }
    }

    public  IShotStrategy GetShotStrategy(ElementType elementType, int id)
    {
        if (bossShotStrategies.ContainsKey((elementType, id)))
        {
            return bossShotStrategies[(elementType, id)];
        }
        return bossShotStrategies[(ElementType.Normal, 0)];
    }
}
