using System;

public interface IPassive
{
    public void Init(TowerManager towerManager , BasePlanet basePlanet);
    public void ApplyPassive(PassiveSystem passiveSystem);
}