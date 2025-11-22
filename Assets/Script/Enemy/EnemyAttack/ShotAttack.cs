using System.Collections.Generic;
using UnityEngine;

public class ShotAttack : IAttack
{
    public bool isAttackColliderOn => false;
    
    Dictionary<ElementType,IShotStrategy> shotStrategies = new Dictionary<ElementType, IShotStrategy>()
    {
        { ElementType.Normal, new NormalStrategy() },
        { ElementType.Fire, new HomingShot() },
        { ElementType.Steel, new NormalStrategy() },
        { ElementType.Ice, new NormalStrategy() },
        { ElementType.Light, new NormalStrategy() },
        { ElementType.Dark, new NormalStrategy() },
    };
    //동일 
    public void Attack(Enemy enemy)
    {
        enemy.attackInterval += Time.deltaTime;
  Debug.Log("Shot Attack");
  Debug.Log($"Attack Interval: {enemy.attackInterval}, Fire Interval: {enemy.fireInterval}");
        if (enemy.attackInterval >= enemy.fireInterval)
        {
            shotStrategies[enemy.ElementType].Shot(enemy, enemy.GetTarget());
            enemy.attackInterval = 0f;
        }
    }

    private void Shot(Enemy enemy, GameObject target)
    {
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.transform.position = enemy.transform.position;
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);
    }

    private EnemyProjectileBase CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<EnemyProjectileBase>(poolsId);
        EnemyProjectileBase projectile = projectileObj.GetComponent<EnemyProjectileBase>();
        return projectile;
    }   
}
