using UnityEngine;


public class TurnSimpleAttack : IShotStrategy
{
    private Enemy enemy;
    private BossPartner bossPartner;
    private bool isBossTurn = true;

    public bool IsBossTurn() => isBossTurn;

    public void Init(Enemy enemy)
    {
        bossPartner = Managers.ObjectPoolManager.SpawnObject<BossPartner>(PoolsId.BossPartner);
        if (bossPartner != null)
        {
            bossPartner.Init(enemy);
        }
    }


    public void Shot(Enemy enemy, GameObject target)
    {
        if (bossPartner == null) return;
        if (!isBossTurn) return;
        if (target == null) return;
        if (enemy == null) return;
        this.enemy = enemy;
  
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);

        isBossTurn = false;
        bossPartner?.EnableAttackTurn(); // 파트너 턴 시작
    }

    public void OnPartnerAttackComplete()
    {
        isBossTurn = true;
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
            flash.transform.position = enemy.transform.position + dir.normalized * (enemy.transform.localScale.x* 0.5f);
            projectile.transform.position = flash.transform.position;   
        }

        return projectile;
    }

    
}
