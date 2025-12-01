using UnityEngine;

public class RapidFireAttack : IShotStrategy
{
    private Enemy body;
    private float baseAngle = 90f;
    public int spawnCount = 10;

    public void Shot(Enemy enemy, GameObject target)
    {
        body = enemy;
        Vector3 toTarget = (target.transform.position - enemy.transform.position).normalized;

        for (int i = 0; i < spawnCount; i++)
        {
            float rnadomAngle = Random.Range(-baseAngle, baseAngle);
            Vector2 dir = RotateVector(toTarget, rnadomAngle);

            var Bullet = CreateProjectile(PoolsId.SwitchDirectionBullet);
            Bullet.transform.position = enemy.transform.position;
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetTarget(target.transform);
            Bullet.SetDirection(dir);
        }
    }

    private SwitchDirectionBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<SwitchDirectionBullet>(poolsId);
        SwitchDirectionBullet projectile = projectileObj.GetComponent<SwitchDirectionBullet>();
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
