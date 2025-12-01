public class LightElemetDamageUpgradeOption : RandomOptionBase
{
    public override string GetOptionStringFormatting()
    {
        return "빛속성 공격력 증가";
    }

    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    public override void ResetRandomOption()
    {
        if(towers == null) return;
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == null) continue;
            if (towers[i].GetElementType() == ElementType.Light)
            {
                towers[i].MinusBonusDamageToPercent(FullOptionValue / 100f);
            }
        }
    }

    public override void SetRandomOption()
    {
        GetApplyOptionTowers();
        if(towers == null)
        {
            return;
        }
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == null) continue;
            if (towers[i].GetElementType() == ElementType.Light)
            {
                towers[i].AddBonusDamageToPercent(FullOptionValue / 100f);
            }
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new LightElemetDamageUpgradeOption();
    }
}

