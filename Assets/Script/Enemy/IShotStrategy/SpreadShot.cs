using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : IShotStrategy
{

    Enemy body;
    int numberOfProjectiles = 3;

    float baseAngle = 90f;

    List<Vector3> spreadAngles = new List<Vector3>();

    public void Shot(Enemy enemy, GameObject target)
    {
        body = enemy;
        Vector3 dir = (target.transform.position - enemy.transform.position).normalized;
        SetSpreadAngle(dir);
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            var Bullet = CreateProjectile(PoolsId.SpreadBullet);
            Bullet.transform.position = enemy.transform.position;
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetTarget(target.transform);
            Bullet.SetDirection(spreadAngles[i]);             
        }
    }

    private SpreadBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<SpreadBullet>(poolsId);
        SpreadBullet projectile = projectileObj.GetComponent<SpreadBullet>();        
        return projectile;
    }

    private void SetSpreadAngle(Vector3 angle)
    {
        spreadAngles.Clear();
        float scaleFactor = (body.transform.localScale.x + body.transform.localScale.y) / 2f;
        float totalSpreadAngle = baseAngle * scaleFactor;
        float angleStep = totalSpreadAngle / (numberOfProjectiles - 1);
        float startAngle = -totalSpreadAngle / 2f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float addAngle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, addAngle);
            Vector3 rotatedDirection = rotation * angle;
            spreadAngles.Add(rotatedDirection.normalized);
        }
    }
}
