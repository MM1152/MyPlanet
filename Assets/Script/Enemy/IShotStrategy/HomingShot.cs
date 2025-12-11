using UnityEngine;

public class HomingShot : IShotStrategy
{
    private Enemy enemy;
    private int homingCount => enemy.enemyType != EnemyType.EliteMonster ? 1 : DataTableManager.OptionTable.GetValueDataToInt(5017); 
    
    public void Shot(Enemy enemy, GameObject target)
    { 
        this.enemy = enemy;
        
        for (int i = 0; i < homingCount; i++)
        {
            var Bullet = CreateProjectile(PoolsId.HomingBullet);
            Bullet.transform.position = enemy.transform.position;
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetTarget(target.transform);
        }
    }
 
    private EnemyProjectileBase CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<EnemyProjectileBase>(poolsId);
        EnemyProjectileBase projectile = projectileObj.GetComponent<EnemyProjectileBase>();
        return projectile;
    }
}
