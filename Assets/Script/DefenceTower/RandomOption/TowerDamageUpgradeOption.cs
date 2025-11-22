using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TowerDamageUpgradeOption : RandomOptionBase
{
    public override void Init(TowerManager towerManager, TowerTable.Data baseTowerData, RandomOptionData.Data optionData)
    {
        base.Init(towerManager, baseTowerData , optionData);

    }

    public override void ResetRandomOption()
    {
        foreach(var tower in towers)
        {
            tower.AddBonusDamage((int)-baseTowerData.optionValue);
        }
    }

    public override void SetRandomOption()
    {
        GetApplyOptionTowers();

        foreach (var tower in towers)
        {
#if DEBUG_MODE
            //Debug.Log($"Before Damage Amount Apply : {tower.Damage} towerID : {tower.ID}");
#endif
            if (tower == null) continue;
            tower.AddBonusDamage((int)baseTowerData.optionValue);
#if DEBUG_MODE
            //Debug.Log($"After Damage Amount Apply : {tower.Damage} towerID : {tower.ID}");
#endif
        }
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new TowerDamageUpgradeOption();
    }

    public override string GetOptionStringFormatting()
    {
        if (optionData.id == 1)
            return string.Empty;
        else
            return string.Empty;
    }
}