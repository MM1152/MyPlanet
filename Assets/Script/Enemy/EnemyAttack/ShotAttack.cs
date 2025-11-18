using System.Runtime.CompilerServices;
using UnityEngine;

public class ShotAttack : IAttack
{
    public bool isAttackColliderOn => false;

    public void Attack(Enemy enemy)
    {
        enemy.attackInterval += Time.deltaTime;

        if (enemy.attackInterval >= enemy.enemyData.AttackInterval)
        {
            Shot(enemy, enemy.GetTarget());
            enemy.attackInterval = 0f;
        }
    }
    private void Shot(Enemy enemy, GameObject target)
    {
        SimpleBullet simpleBullet = CreateProjectile(PoolsId.SimpleBullet) as SimpleBullet;
        simpleBullet.transform.position = enemy.transform.position;
        simpleBullet.Init(enemy, enemy.typeEffectiveness);
        simpleBullet.SetTarget(target.transform);   
    }

    private EnemyProjectileBase CreateProjectile( PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<EnemyProjectileBase>(poolsId);
        EnemyProjectileBase projectile = projectileObj.GetComponent<EnemyProjectileBase>();        
        return projectile;
    }
}
