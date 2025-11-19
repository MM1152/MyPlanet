using System;

public interface IPassive
{
    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData , PassiveSystem passiveSystem);
    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy);
    public IPassive CreateInstance();
}