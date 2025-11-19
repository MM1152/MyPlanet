public interface ICondition
{
    public void Init(PassiveTable.Data passiveData , EffectTable.Data effectData);
    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy);
    public ICondition CreateInstance();
}
