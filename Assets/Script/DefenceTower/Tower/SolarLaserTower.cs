using UnityEngine;
using System.Collections.Generic;
public class SolarLaserTower : Tower
{
    // 애는 소환해놓고 아무것도 안해야됨 레이저단에서 관리해야될거같음
    // 레벨업 될때만 갯수 위치 정해주고 사용하는 방식으로 이용되어야할거같음
    private List<SolarLaser> solarLaser = new List<SolarLaser>();
    public override bool Attack(bool useTarget = true)
    {
        return false;
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }

    public override void LevelUp(LevelUpTable.Data levelUpData)
    {
        base.LevelUp(levelUpData);
        if(solarLaser.Count != BonusProjectileCount)
        {
            for(int i = solarLaser.Count; i < BonusProjectileCount; i++)
            {
                var laser = Managers.ObjectPoolManager.SpawnObject<SolarLaser>(PoolsId.SolarLaser);
                laser.Init(this);
                solarLaser.Add(laser);
            }
        }
        LaserUpdate();
    }

    public override void Update(float deltaTime)
    {
        return;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        return null;
    }


    private void LaserUpdate()
    {
        float angle = 360f / solarLaser.Count;
        for(int i = 0; i < solarLaser.Count; i++)
        {
            solarLaser[i].UpdateLaser(angle * i);
            solarLaser[i].UpgradeLaser();
        }
    }

    public override void PlaceTower()
    {
        base.PlaceTower();
        for(int i = 0; i < solarLaser.Count; i++) 
        {
            solarLaser[i].UpgradeLaser();
        }
    }

    public override void UnPlaceTower()
    {
        base.UnPlaceTower();
        for(int i = 0; i < solarLaser.Count; i++) 
        {
            solarLaser[i].UpgradeLaser();
        }
    }
}