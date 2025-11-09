using UnityEngine;
using System.Collections.Generic;
using System;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private EnemyTest testTarget;
    [SerializeField] private GameObject tower;
    private List<Tower> towers = new List<Tower>();
    private Dictionary<int, Tower> towerTable = new Dictionary<int, Tower>();
    
    private void Awake()
    {
        MuchineGunTower muchineGunTower = new MuchineGunTower();
        MissileTower missileTower = new MissileTower();
        LaserTower laserTower = new LaserTower();

        muchineGunTower.Init(tower, this , new TowerData.Data() { name = "MuchineGunTower", attackRadius = 1.5f, attackInterval = 1f});
        missileTower.Init(tower, this, new TowerData.Data() { name = "MissileTower", attackRadius = 1.5f, attackInterval = 1.5f});
        laserTower.Init(tower, this, new TowerData.Data() { name = "LaserTower" , attackRadius = 1.5f, attackInterval = 2f});

        towerTable.Add(1, muchineGunTower);
        towerTable.Add(2 , missileTower);
        towerTable.Add(3 , laserTower);
    }

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
        towers.Add(towerTable[3]);
    }
}
