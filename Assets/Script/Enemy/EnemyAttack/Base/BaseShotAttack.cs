using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShotAttack : IAttack
{
    public bool isAttackColliderOn => false;

    protected Dictionary<ElementType, IShotStrategy> shotStrategies;
    public virtual void Attack(Enemy enemy)
    {
        enemy.attackInterval += Time.deltaTime;

        var strategy = GetShotStrategy(enemy.ElementType);
        
        if (strategy is LaserShot laserShot)
        {
            laserShot.LaserUpdate(enemy, enemy.GetTarget());
        }
      

        if (enemy.attackInterval >= enemy.fireInterval)
        {
            strategy.Shot(enemy, enemy.GetTarget());
            enemy.attackInterval = 0f;
            enemy.ReturnMoveAction?.Invoke();
        }
    }
    public IShotStrategy GetShotStrategy(ElementType elementType)
    {
        if (shotStrategies.ContainsKey(elementType))
        {
            return shotStrategies[elementType];
        }
        return shotStrategies[ElementType.Normal];
    }
}