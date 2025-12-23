using UnityEngine;

public class NormalStrategy : IShotStrategy
{
   private Enemy enemy;
   
    public void Shot(Enemy enemy, GameObject target)
    {
        this.enemy = enemy;
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);
    }

    private SimpleBullet CreateProjectile(PoolsId poolsId)
    { 
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<SimpleBullet>(poolsId);
        SimpleBullet projectile = projectileObj.GetComponent<SimpleBullet>();
        if(enemy.target != null)
        {
            var dir = enemy.target.transform.position - enemy.transform.position;

            projectile.SetHitParticle(PoolsId.Hit13redlaser);
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
            flash.transform.position = enemy.transform.position + dir.normalized * enemy.transform.localScale.x;
            projectile.transform.position = flash.transform.position;
        }

        return projectile;
    }
}
