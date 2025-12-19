using UnityEngine;

public class MissileRainAttack : IShotStrategy
{
    private Enemy enemy;
    private float baseAngle = 90f;
    public int spawnCount = 4;
   
    public void Shot(Enemy enemy, GameObject target)
    {        
        this.enemy = enemy;
        Vector3 toTarget = -((target.transform.position - enemy.transform.position).normalized);

        for (int i = 0; i < spawnCount; i++)
        {
            float rnadomAngle = Random.Range(-baseAngle, baseAngle);
            Vector2 dir = RotateVector(toTarget, rnadomAngle);

            var Bullet = CreateProjectile(PoolsId.RainBullet, dir);
            Bullet.transform.position = enemy.transform.position;
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetRectLind(enemy);
            Bullet.SetTarget(target.transform);
            Bullet.SetDirection(dir);
        }
    }

    private RainBullet CreateProjectile(PoolsId poolsId, Vector2 dir)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<RainBullet>(poolsId);
        RainBullet projectile = projectileObj.GetComponent<RainBullet>();
         if(enemy.target != null)
        {
            projectile.SetHitParticle(PoolsId.Hit13redlaser);
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
            flash.transform.position = enemy.transform.position + (Vector3)dir.normalized * (enemy.transform.localScale.x * 0.5f);
            projectile.transform.position = flash.transform.position;
        }
        return projectile;
    }

    private Vector2 RotateVector(Vector2 dir, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            dir.x * cos - dir.y * sin,
            dir.x * sin + dir.y * cos
        ).normalized;
    }

}
