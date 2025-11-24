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
        return base.Attack(useTarget);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet projectile = Managers.ObjectPoolManager.SpawnObject<Bullet>(PoolsId.Bullet);
        //Bullet projectile = GameObject.Instantiate(attackprefab).GetComponent<Bullet>();
        return projectile;
    }
}
