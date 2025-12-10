using UnityEngine;
using System.Collections.Generic;

public class DroneTower : UtilTower
{
    private List<Drone> drones = new List<Drone>();

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }

    public override bool Attack(bool useTarget = false)
    {
        if(drones.Count < BonusDroneCount)
        {
            CreateAttackPrefab();
        }
        else if(drones.Count == BonusDroneCount)
        {
            drones[0].ForceDead();
            CreateAttackPrefab();
        }

        return true;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var drone = Managers.ObjectPoolManager.SpawnObject<Drone>(PoolsId.Drone);
        drone.Init(this);
        drone.transform.position = tower.transform.position;
        drone.OnDead += OnDeadDrone;
        drones.Add(drone);
        return drone;
    }

    private void OnDeadDrone(Drone drone)
    {
        drones.Remove(drone);
        drone.OnDead -= OnDeadDrone;
    }
}