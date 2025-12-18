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
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetTarget(target.transform);
            SetParticlePosition(Bullet,Bullet.OffsetDir);
        }
    }
 
    private HomingBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<HomingBullet>(poolsId);
        HomingBullet projectile = projectileObj.GetComponent<HomingBullet>();
        return projectile;
    }
    private void SetParticlePosition(HomingBullet bullet,Vector2 dir)
    {
        bullet.SetHitParticle(PoolsId.Hit13redlaser);
        var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
        flash.transform.position = enemy.transform.position + (Vector3)dir.normalized * (enemy.transform.localScale.x * 0.5f);     
        bullet.transform.position = flash.transform.position;   
    }
}
