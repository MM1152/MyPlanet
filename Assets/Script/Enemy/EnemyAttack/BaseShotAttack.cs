using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShotAttack : IAttack
{
    public bool isAttackColliderOn => false;

    protected Dictionary<ElementType, IShotStrategy> shotStrategies;
    public void Attack(Enemy enemy)
    {
        enemy.attackInterval += Time.deltaTime;

        if (GetShotStrategy(enemy.ElementType) is LaserShot laserShot)
        {
            laserShot.laserUpdate(enemy, enemy.GetTarget());
        }

        if (enemy.attackInterval >= enemy.fireInterval)
        {
            GetShotStrategy(enemy.ElementType).Shot(enemy, enemy.GetTarget());
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