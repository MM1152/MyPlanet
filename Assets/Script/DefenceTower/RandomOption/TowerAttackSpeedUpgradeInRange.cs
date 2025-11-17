using System.Collections.Generic;

public class TowerAttackSpeedUpgradeInRange : RandomOptionBase
{
    private List<Tower> towers;
    public override string GetOptionStringFormatting()
    {
        return string.Format(optionData.description , baseTowerData.Option_Range , baseTowerData.optionValue);
    }

    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData, optionData);
    }

    private void GetApplyOptionTowers()
    {
        towers = towerManager.GetAroundTower(baseTowerData, baseTowerData.Option_Range);
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
        return new TowerAttackSpeedUpgradeInRange();
    }
}

