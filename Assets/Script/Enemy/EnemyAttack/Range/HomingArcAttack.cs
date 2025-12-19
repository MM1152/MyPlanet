using UnityEngine;
using System.Collections.Generic;
using System;

public class HomingArcAttack : IShotStrategy
{
    private Enemy enemy;
    private float baseAngle = 135f;
    public int spawnCount = 4;
    private Action<GameObject> shotPattern;
    private bool initialized = false;
    private List<Vector3> spreadAngles = new List<Vector3>();

    public void Shot(Enemy enemy, GameObject target)
    {
        if (!initialized)
        {
            this.enemy = enemy;
            InitializePattern(enemy.enemyData.ID);
        }

        shotPattern?.Invoke(target);
    }

    private void InitializePattern(int enemyId)
    {
        shotPattern = enemyId switch
        {
            3042 => ShotSpread,
            3062 => ShotAlternate,
            _ => ShotSpread
        };
        initialized = true;
    }

    // 3042: 흩뿌리기
    private void ShotSpread(GameObject target)
    {
        Vector3 dir = (target.transform.position - enemy.transform.position).normalized;
        SetSpreadAngle(dir);
        for (int i = 0; i < spawnCount; i++)
        {
            var Bullet = CreateProjectile(PoolsId.ArcMissileBullet, spreadAngles[i]);
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

    private void ShotAlternate(GameObject target)
    {
        Vector3 dir = (target.transform.position - enemy.transform.position).normalized;
        SetSpreadAngle(dir);

        Vector3 chosenDirection;
        var randomNum = UnityEngine.Random.Range(0f, 1f);
        chosenDirection = randomNum < 0.5f ? spreadAngles[0] : spreadAngles[spawnCount - 1];

        var Bullet = CreateProjectile(PoolsId.ArcMissileBullet, chosenDirection);
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);
        Bullet.SetDirection(chosenDirection);
        Bullet.SetTurnSpeed(1f);
    }

    private ArcMissileBullet CreateProjectile(PoolsId poolsId,Vector3 dir)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<ArcMissileBullet>(poolsId);
        ArcMissileBullet projectile = projectileObj.GetComponent<ArcMissileBullet>();
        if (enemy.target != null)
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
