using System.Collections.Generic;
using UnityEngine;

public abstract class RandomOptionBase
{
    protected BasePlanet planet;
    protected TowerManager towerManager;
    protected TowerTable.Data baseTowerData;
    protected RandomOptionData.Data optionData;
    protected List<Tower> towers;
    // Deep Copy 된 대상에서 사용하기
    public virtual void Init(TowerManager towerManager, TowerTable.Data baseTowerData , RandomOptionData.Data optionData)
    {
        this.optionData = optionData;
        this.towerManager = towerManager;
        this.planet = GameObject.FindWithTag("Player").GetComponent<BasePlanet>();
        this.baseTowerData = baseTowerData;
    }

    public RandomOptionBase DeepCopy()
    {
        RandomOptionBase copyRandomOption = CreateInstance();
        return copyRandomOption;
    }

    protected void GetApplyOptionTowers()
    {
        int side = baseTowerData.Option_type;

        if (side == 0)
        {
            towers = towerManager.GetAroundTower(baseTowerData, baseTowerData.Option_Range);
        }
        else if (side == 1)
        {
            towers = towerManager.GetLeftTower(baseTowerData, baseTowerData.Option_Range);
        }
        else if (side == 2)
        {
            towers = towerManager.GetLeftTower(baseTowerData, baseTowerData.Option_Range);
        }
    }

    protected abstract RandomOptionBase CreateInstance();
    public abstract void ResetRandomOption();
    public abstract void SetRandomOption();
    public abstract string GetOptionStringFormatting();
}