using UnityEngine;
using System.Collections.Generic;
public class HomingArcAttack : IShotStrategy
{
    private Enemy body;
    private float baseAngle =135f;
    public int spawnCount = 4;
    private List<Vector3> spreadAngles = new List<Vector3>();

    public void Shot(Enemy enemy, GameObject target)
    {
        body = enemy;
        Vector3 dir = (target.transform.position - enemy.transform.position).normalized;
        SetSpreadAngle(dir);
        for (int i = 0; i < spawnCount; i++)
        {
            var Bullet = CreateProjectile(PoolsId.ArcMissileBullet);
            Bullet.transform.position = enemy.transform.position;
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetTarget(target.transform);
            Bullet.SetDirection(spreadAngles[i]);
            
            if (i == 0 || i == spawnCount - 1)
            {
                Bullet.SetTurnSpeed(1f); 
            }
            else
            {
                Bullet.SetTurnSpeed(0.3f);
            }
        }
    }

    private ArcMissileBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<ArcMissileBullet>(poolsId);
        ArcMissileBullet projectile = projectileObj.GetComponent<ArcMissileBullet>();
        return projectile;
    }

    private void SetSpreadAngle(Vector3 angle)
    {
        spreadAngles.Clear();
     
        float totalSpreadAngle = baseAngle; 
        float angleStep = totalSpreadAngle / (spawnCount - 1);
        float startAngle = -totalSpreadAngle / 2f;

        for (int i = 0; i < spawnCount; i++)
        {
            float addAngle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, addAngle);
            Vector3 rotatedDirection = rotation * angle;
            spreadAngles.Add(rotatedDirection.normalized);
        }
    }


}
