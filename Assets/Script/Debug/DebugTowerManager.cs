using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

public class DebugTowerManager : TowerManager
{
    protected override void Awake()
    {
        List<TowerTable.Data> towerDatas = DataTableManager.TowerTable.GetAll();

        for(int i = 0; i < towerDatas.Count; i++)
        {
            AddTower(towerDatas[i] , i + 1);
        }
    }

    public override void AddTower(TowerTable.Data data, int slotIndex)
    {
        Tower instanceTower = towerFactory.CreateInstance(data.ID);
        towers.Add(instanceTower);
        instanceTower.Init(tower, this, data, slotIndex);
        instanceTower.SetPlanetData(basePlanet.PlanetData);
    }

    public override void PlaceTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);
        towers[index].PlaceTower();
    }

    public void LevelUpTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);
        var levelUpData = DataTableManager.LevelUpTable.Get(towerData.ID, towers[index].Level + 1);
        towers[index].LevelUp(levelUpData);
    }

    public void UnPlaceTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);
        towers[index].UnPlaceTower();
    }
}
