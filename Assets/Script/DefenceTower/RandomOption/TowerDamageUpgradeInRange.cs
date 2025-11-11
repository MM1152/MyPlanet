using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TowerDamageUpgradeInRange : RandomOptionBase
{
    private List<Tower> towers;

    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData , optionData);
    }

    private void GetApplyOptionTowers()
    {
        towers = towerManager.GetAroundTower(baseTowerData, baseTowerData.Option_Range);
    }

    public override void ResetRandomOption()
    {
        foreach(var tower in towers)
        {
            tower.AddBonusDamage(-baseTowerData.optionValue);
        }
    }

    public override void SetRandomOption()
    {
        GetApplyOptionTowers();
        foreach (var tower in towers)
        {
#if DEBUG_MODE
            Debug.Log($"Before Damage Amount Apply : {tower.Damage} towerID : {tower.ID}");
#endif
            tower.AddBonusDamage(baseTowerData.optionValue);
#if DEBUG_MODE
            Debug.Log($"After Damage Amount Apply : {tower.Damage} towerID : {tower.ID}");
#endif
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new TowerDamageUpgradeInRange();
    }

    public override string GetOptionStringFormatting()
    {
        if (optionData.id == 1)
            return string.Format(optionData.description, baseTowerData.Option_Range, baseTowerData.optionValue);
        else
            return string.Format(optionData.description, baseTowerData.optionValue);
    }
}