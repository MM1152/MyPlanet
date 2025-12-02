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
            { (ElementType.Ice, 0), new NormalStrategy() },
            { (ElementType.Steel, 3027), new RapidFireAttack() },
            { (ElementType.Steel, 3032), new MissileRainAttack() },
            { (ElementType.Light, 0), new NormalStrategy() },
            { (ElementType.Dark, 0), new NormalStrategy() },
        };
    }
    public override void Attack(Enemy enemy)
    {
        enemy.attackInterval += Time.deltaTime;

        if (enemy.attackInterval >= enemy.fireInterval)
        {
            GetShotStrategy(enemy.ElementType,enemy.enemyData.ID).Shot(enemy, enemy.GetTarget());
            enemy.attackInterval = 0f;
            enemy.ReturnMoveAction?.Invoke();
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
