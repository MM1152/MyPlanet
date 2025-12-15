using UnityEngine;
using System;

public class ShockWaveTower : UtilTower
{
    public override bool Attack(bool useTarget = true)
    {
        CreateAttackPrefab();
        return true;
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var shockWave = Managers.ObjectPoolManager.SpawnObject<ShockWave>(PoolsId.ShockWave);
        shockWave.Init(this);
        shockWave.SetFollowTarget(tower.transform);

        return shockWave;
    }
}