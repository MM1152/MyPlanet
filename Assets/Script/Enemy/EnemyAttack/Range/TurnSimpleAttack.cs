using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TurnSimpleAttack : IShotStrategy
{
    private BossPartner bossPartner;
    private bool isBossTurn = true;

    public void SetBossPartner(BossPartner partner)
    {
        bossPartner = partner;
    }

    public bool IsBossTurn() => isBossTurn;

    public void Shot(Enemy enemy, GameObject target)
    {
        if (!isBossTurn) return;
        if (target == null) return;
        if (enemy == null) return;
  
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.transform.position = enemy.transform.position;
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);

        isBossTurn = false;
        bossPartner?.EnableAttackTurn(); // 파트너 턴 시작
    }

    public void OnPartnerAttackComplete()
    {
        isBossTurn = true;
    }

    private EnemyProjectileBase CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<EnemyProjectileBase>(poolsId);
        EnemyProjectileBase projectile = projectileObj.GetComponent<EnemyProjectileBase>();
        return projectile;
    }

    
}
