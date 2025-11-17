public class BasePlanetUpgradeMaxHp : RandomOptionBase
{
    public override string GetOptionStringFormatting()
    {
        return string.Format(optionData.description, baseTowerData.optionValue);
    }

    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    public override void ResetRandomOption()
    {
        planet.maxHp -= baseTowerData.optionValue;
    }

    public override void SetRandomOption()
    {
        planet.maxHp += baseTowerData.optionValue;
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new BasePlanetUpgradeMaxHp();
    }
}