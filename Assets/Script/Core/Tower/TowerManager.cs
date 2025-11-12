using UnityEngine;
using System.Collections.Generic;
using System;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private Enemy testTarget;
    [SerializeField] private GameObject tower;
    private List<Tower> towers = new List<Tower>();
    public List<Tower> Towers => towers;
    private TowerFactory towerFactory = new TowerFactory();

    private async void Awake()
    {
        await DataTableManager.WaitForInitalizeAsync();

    }

    public void LateUpdate()
    {
        foreach(var tower in towers)
        {
            tower.Update(Time.deltaTime);
            tower.Attack();
        }
    }

    public Transform FindTarget()
    {
        return testTarget.transform;
    }

    public void AddTower(TowerTable.Data data)
    {
        Tower instanceTower = towerFactory.CreateInstance(data.ID);
        towers.Add(instanceTower);
        instanceTower.Init(tower , this , data);
    }

    public void PlaceTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);

        if (towers[index].UseAble)
        {
            towers[index].LevelUp();
            return;
        }
        towers[index].PlaceTower();
    }

    private int FindTowerPlaceIndex(TowerTable.Data towerData)
    {
        for(int i = 0; i < towers.Count; i++)
        {
            if (towers[i].ID == towerData.ID)
            {
                return i;
            }
        }
        return -1;
    }

    public List<Tower> GetAroundTower(TowerTable.Data towerData, int radius)
    {
        int target = FindTowerPlaceIndex(towerData);
        if(target == -1)
        {
            return null;
        }

        List<Tower> targetedTowres = new List<Tower>();
#if DEBUG_MODE
        //Debug.Log($"targetIndex : {target} , range : {radius}");
#endif
        for(int i = 1; i <= radius; i++)
        {
            int leftPointer = Utils.ClampIndex(target - i , towers.Count);
            int rightPointer = Utils.ClampIndex(target + i, towers.Count);
            Debug.Log($"leftPointer : {leftPointer} , rightPointer : {rightPointer}");
            targetedTowres.Add(towers[leftPointer]);
            targetedTowres.Add(towers[rightPointer]);
        }

        return targetedTowres;
    }

    public Tower GetRandomTower()
    {
        int rand = UnityEngine.Random.Range(0, towers.Count);
        return towers[rand];
    }
}
