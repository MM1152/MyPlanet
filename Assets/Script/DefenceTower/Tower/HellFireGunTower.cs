using UnityEngine;
using System;

public class HellFireGunTower : Tower
{
    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data , int slotIndex)
    {
        base.Init(tower, manager, data , slotIndex);
    }

    public override bool Attack(bool useTarget = true)
    { 
        Target = manager.FindTarget()?.transform;
        base.Attack(useTarget);

        return true;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet projectile = Managers.ObjectPoolManager.SpawnObject<HellFireBullet>(PoolsId.HellFireBullet);

        if(Target != null)
        {
            var dir = target.transform.position - TowerGameObject.transform.position;

            projectile.SetHitSound(AudiosId.Hit_8);
            projectile.SetHitParticle(PoolsId.Hit13redlaser);
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
            flash.transform.position = TowerGameObject.transform.position + dir.normalized * TowerGameObject.transform.localScale.x;
        }

        //Bullet projectile = GameObject.Instantiate(attackprefab).GetComponent<Bullet>();
        return projectile;
    }
}
