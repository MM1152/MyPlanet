using System;
using System.Collections.Generic;
using UnityEngine;
public class PassiveSystem
{
    private IPassive passive;
    private PlanetTable.Data planetData;

    public Action<float> OnUpdate;
    public Action OnHit;
    public Action OnAttack;
    public Action OnInit;

    private readonly Dictionary<int, IPassive> passiveMap = new Dictionary<int, IPassive>()
    {
        { 1001 , new EarthPassive() },
    };

    public void Init(PlanetTable.Data planetData , TowerManager towerManager , BasePlanet planet)
    {
        this.planetData = planetData;
        if(passiveMap.ContainsKey(planetData.ID))
        {
            this.passive = passiveMap[planetData.ID];
            this.passive.Init(towerManager, planet);
            this.passive.ApplyPassive(this);
        }
        else
        {
            Debug.Log("해당 행성은 패시브가 존재하지 않습니다.");
        }
    }

    public void Update(float deltaTime)
    {
        OnUpdate?.Invoke(deltaTime);
    }
}