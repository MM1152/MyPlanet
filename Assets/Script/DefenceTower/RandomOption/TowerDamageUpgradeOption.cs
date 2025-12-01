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
        if(towers == null) return;
        foreach(var tower in towers)
        {
            if (tower == null) continue;
            tower.MinusBonusDamageToPercent(FullOptionValue / 100f);
        }
    }

    public override void SetRandomOption()
    {
        GetApplyOptionTowers();
        if(towers == null)
        {
            return;
        }

        foreach (var tower in towers)
        {
#if DEBUG_MODE
            //Debug.Log($"Before Damage Amount Apply : {tower.Damage} towerID : {tower.ID}");
#endif
            if (tower == null) continue;
            tower.AddBonusDamageToPercent(FullOptionValue / 100f);
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
        return "공격력 증가";
    }
}