public class SteelElemetDamageUpgradeOption : RandomOptionBase
{
    public override string GetOptionStringFormatting()
    {
        return "금속속성 공격력 증가";
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
            if (towers[i].GetElementType() == ElementType.Steel)
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
            if (towers[i].GetElementType() == ElementType.Steel)
            {
                towers[i].AddBonusDamageToPercent(FullOptionValue / 100f);
            }
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new SteelElemetDamageUpgradeOption();
    }
}

