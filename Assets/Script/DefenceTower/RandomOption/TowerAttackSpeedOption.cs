using System.Collections.Generic;

public class TowerAttackSpeedOption : RandomOptionBase
{
    public override string GetOptionStringFormatting()
    {
        return "공격속도 증가";
    }

    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    public override void ResetRandomOption()
    {
        if(towers == null) return;
        foreach (var applyTower in towers)
        {
            if (applyTower == null) continue;
            applyTower.MinusBonusAttackSpeedTopercent(FullOptionValue / 100f);
        }
    }

    public override void SetRandomOption()
    {
        GetApplyOptionTowers();
        if(towers == null)
        {
            return;
        }
        foreach(var applyTower in towers)
        {
            if (applyTower == null) continue;
            applyTower.AddBonusAttackSpeedTopercent(FullOptionValue / 100f);
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new TowerAttackSpeedOption();
    }
}

