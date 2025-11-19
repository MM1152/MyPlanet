using UnityEngine;
using System;

public class HellFireGunTower : Tower
{
    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data , int slotIndex)
    {
        base.Init(tower, manager, data , slotIndex);
        minNoise = -0.05f;
        maxNoise = 0.05f;
    }

    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    public override void LevelUp()
    {
        base.LevelUp();
        bonusAttackSpeed += 0.1f;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet projectile = Managers.ObjectPoolManager.SpawnObject<Bullet>(PoolsId.Bullet);
        //Bullet projectile = GameObject.Instantiate(attackprefab).GetComponent<Bullet>();
        return projectile;
    }
}
