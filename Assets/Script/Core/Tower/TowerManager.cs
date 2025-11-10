using UnityEngine;
using System.Collections.Generic;
using System;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private EnemyTest testTarget;
    [SerializeField] private GameObject tower;
    private List<Tower> towers = new List<Tower>();

    public void Update()
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

    public void AddTower(TowerData.Data data)
    {
        if(towers.Contains(data.tower))
        {
            data.tower.LevelUp();
            return;
        }
        towers.Add(data.tower);
        data.tower.Init(tower , this , data);
    }
}
