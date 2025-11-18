using System;
using UnityEngine;

public class EarthPassive : IPassive
{
    private TowerManager towerManager;
    private BasePlanet basePlanet;
    private float passiveTime = 3f;
    private float timer = 0f;
    public void ApplyPassive(PassiveSystem passiveSystem)
    {
        passiveSystem.OnUpdate = EarthPassiveEffect;
    }

    public void Init(TowerManager towerManager, BasePlanet basePlanet)
    {
        this.towerManager = towerManager;
        this.basePlanet = basePlanet;
    }

    private void EarthPassiveEffect(float deltaTime)
    {
        float percent = basePlanet.hp / basePlanet.maxHp;
        if(percent < 0.5f)
        {
            timer += deltaTime;
            if(passiveTime <= timer)
            {
                basePlanet.RepairHp(50);
                Debug.Log("Hp 회복");
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
        }
    }
}
