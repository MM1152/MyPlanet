public class FireElemetDamageUpgradeOption : RandomOptionBase
{
    public override string GetOptionStringFormatting()
    {
        return string.Empty;
    }

    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    public override void ResetRandomOption()
    {

    }

    public override void SetRandomOption()
    {

    }

    protected override RandomOptionBase CreateInstance()
    {
        return new FireElemetDamageUpgradeOption();
    }
}

