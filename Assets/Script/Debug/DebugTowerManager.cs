using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

public class DebugTowerManager : TowerManager
{
    private Tower currentPlaceTower;
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
        if (instanceTower == null) return;

        towers.Add(instanceTower);
        instanceTower.Init(tower, this, data, slotIndex);
        instanceTower.SetPlanetData(basePlanet.PlanetData);
        instanceTower.LevelUp(DataTableManager.LevelUpTable.Get(data.ID, instanceTower.Level + 1));

    }

    public override void PlaceTower(TowerTable.Data towerData)
    {
        if (currentPlaceTower != null)
            currentPlaceTower.UnPlaceTower();

        int index = FindTowerPlaceIndex(towerData);
        currentPlaceTower = towers[index];
        currentPlaceTower.PlaceTower();
    }

    public void LevelUpTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);
        var levelUpData = DataTableManager.LevelUpTable.Get(towerData.ID, towers[index].Level + 1);
        if(levelUpData == null)
            return;
        towers[index].LevelUp(levelUpData);
    }

    public void LevelDownTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);
        var levelDownData = DataTableManager.LevelUpTable.Get(towerData.ID, towers[index].Level - 1);
        if(levelDownData == null)
            return;
        towers[index].LevelDown(levelDownData);
    }

    public void UnPlaceTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);
        towers[index].UnPlaceTower();
    }
}
