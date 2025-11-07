using UnityEngine;
using System.Collections.Generic;
using System;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private EnemyTest testTarget;
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
        var muchine = new MuchineGunTower();
        muchine.Init(this, data);
        towers.Add(muchine);
    }
}
