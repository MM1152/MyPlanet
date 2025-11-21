using System;
using UnityEngine;

public class GravityControlTower : Tower
{
    public override bool Attack()
    {
        if (attackAble)
        {
            attackAble = false;
            currentAttackInterval = 0;

            Debug.Log($"Tower Attack ");

            BaseAttackPrefab attackPrefabs = CreateAttackPrefab();
            attackPrefabs.transform.position = manager.basePlanet.transform.position;
            attackPrefabs.Init(this);
            attackPrefabs.SetTarget(target, noise);
            
            return true;
        }

        return false;
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);

        //Fix : 임시값이라 변경 필요
        statusEffect = new SlowStatusEffect(2f, 0.5f);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        return Managers.ObjectPoolManager.SpawnObject<GravityControl>(PoolsId.GravityControl);
    }
}