using UnityEngine;

public class MissileRainAttack : IShotStrategy
{
    private float baseAngle = 90f;
    public int spawnCount = 4;
   
    public void Shot(Enemy enemy, GameObject target)
    {        
        Vector3 toTarget = -((target.transform.position - enemy.transform.position).normalized);

        for (int i = 0; i < spawnCount; i++)
        {
            float rnadomAngle = Random.Range(-baseAngle, baseAngle);
            Vector2 dir = RotateVector(toTarget, rnadomAngle);

            var Bullet = CreateProjectile(PoolsId.RainBullet);
            Bullet.transform.position = enemy.transform.position;
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetRectLind(enemy);
            Bullet.SetTarget(target.transform);
            Bullet.SetDirection(dir);
        }
    }

    private RainBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<RainBullet>(poolsId);
        RainBullet projectile = projectileObj.GetComponent<RainBullet>();
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
