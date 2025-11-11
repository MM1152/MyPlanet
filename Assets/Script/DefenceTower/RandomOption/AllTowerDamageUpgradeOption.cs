public class AllTowerDamageUpgradeOption : RandomOptionBase
{
    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    public override string GetOptionStringFormatting()
    {
        return string.Format(optionData.description , baseTowerData.optionValue);
    }

    public override void ResetRandomOption()
    {
        for(int i = 0; i < towerManager.Towers.Count; i++)
        {
            towerManager.Towers[i].bonusDamage -= baseTowerData.optionValue;
        }
    }

    public override void SetRandomOption()
    {
        for (int i = 0; i < towerManager.Towers.Count; i++)
        {
            towerManager.Towers[i].bonusDamage += baseTowerData.optionValue;
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new AllTowerDamageUpgradeOption();
    }
}