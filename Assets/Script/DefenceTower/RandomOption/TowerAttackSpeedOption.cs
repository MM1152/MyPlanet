using System.Collections.Generic;

public class TowerAttackSpeedOption : RandomOptionBase
{
    public override string GetOptionStringFormatting()
    {
        return string.Empty;
    }

    public override void Init(TowerManager towerManagr, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    public override void ResetRandomOption()
    {
        foreach (var applyTower in towers)
        {
            applyTower.AddBonusAttackSpeed(-baseTowerData.optionValue / 100f);
        }
    }

    public override void SetRandomOption()
    {
        GetApplyOptionTowers();
        foreach(var applyTower in towers)
        {
            applyTower.AddBonusAttackSpeed(baseTowerData.optionValue / 100f);
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new TowerAttackSpeedOption();
    }
}

