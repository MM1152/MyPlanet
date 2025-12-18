using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpreadShot : IShotStrategy
{
    private Enemy enemy;
    private int baseProjectiles => SetProjectileCount(enemy);

    public int numberOfProjectiles => baseProjectiles + bonus;  
    private int bonus = 0;
    private float baseAngle = 90f;
    private bool bonusApplied = false;
    List<Vector3> spreadAngles = new List<Vector3>();

    private int SetProjectileCount(Enemy body)
    {
        return body.enemyData.ID switch
        {
            3012 => DataTableManager.OptionTable.GetValueDataToInt(5034),
            3022 => DataTableManager.OptionTable.GetValueDataToInt(5038),
            _ => 3,
        };
    }

    public void Shot(Enemy enemy, GameObject target)
    {
        this.enemy = enemy;
      
        Vector3 dir = (target.transform.position - enemy.transform.position).normalized;
        SetSpreadAngle(dir);
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            var Bullet = CreateProjectile(PoolsId.SpreadBullet, spreadAngles[i]);
            Bullet.Init(enemy, enemy.typeEffectiveness);
            Bullet.SetTarget(target.transform);
            Bullet.SetDirection(spreadAngles[i]);
        }
    }

    private SpreadBullet CreateProjectile(PoolsId poolsId,Vector3 dir)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<SpreadBullet>(poolsId);
        SpreadBullet projectile = projectileObj.GetComponent<SpreadBullet>();
         if(enemy.target != null)
        {
            projectile.SetHitParticle(PoolsId.Hit13redlaser);
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
            flash.transform.position = enemy.transform.position + dir.normalized * (enemy.transform.localScale.x * 0.5f);
            projectile.transform.position = flash.transform.position;
        }

        return projectile;
    }

    private void SetSpreadAngle(Vector3 angle)
    {
        spreadAngles.Clear();
        float scaleFactor = (enemy.transform.localScale.x + enemy.transform.localScale.y) / 2f;
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

    public void SetBonusPellet(int bonus)
    {
        if (bonusApplied) return;
        this.bonus += bonus;    
        bonusApplied = true;
    }

    public void ResetPellet()
    {
        bonus = 0;
        bonusApplied = false;
    }
}
