public class AllTowerAttackSpeedUpgrade : RandomOptionBase
{
    public override string GetOptionStringFormatting()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    public override void ResetRandomOption()
    {
        for (int i = 0; i < towerManager.Towers.Count; i++)
        {
            towerManager.Towers[i].AddBonusAttackSpeed(-baseTowerData.optionValue);
        }
    }

    public override void SetRandomOption()
    {
        for(int i = 0; i < towerManager.Towers.Count; i++)
        {
            towerManager.Towers[i].AddBonusAttackSpeed(baseTowerData.optionValue);
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new AllTowerAttackSpeedUpgrade();
    }
}