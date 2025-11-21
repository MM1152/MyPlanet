using UnityEngine;

public class HomingShot : IShotStrategy
{
    public void Shot(Enemy enemy, GameObject target)
    {
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.transform.position = enemy.transform.position;
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);
    }   

    // private void Shot(Enemy enemy, GameObject target)
    // {
    //     var Bullet = CreateProjectile(PoolsId.SimpleBullet);
    //     Bullet.transform.position = enemy.transform.position;
    //     Bullet.Init(enemy, enemy.typeEffectiveness);
    //     Bullet.SetTarget(target.transform);
    // }

    // private EnemyProjectileBase CreateProjectile(PoolsId poolsId)
    // {
    //     var projectileObj = Managers.ObjectPoolManager.SpawnObject<EnemyProjectileBase>(poolsId);
    //     EnemyProjectileBase projectile = projectileObj.GetComponent<EnemyProjectileBase>();
    //     return projectile;
    // }   
}
