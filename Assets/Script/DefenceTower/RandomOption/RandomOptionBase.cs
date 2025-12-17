using System.Collections.Generic;
using UnityEngine;

public abstract class RandomOptionBase
{
    protected BasePlanet planet;
    protected TowerManager towerManager;
    protected TowerTable.Data baseTowerData;
    protected RandomOptionData.Data optionData;
    protected List<Tower> towers;
    protected int level = 0;
    protected int bonusValue = 0;
    public float FullOptionValue => baseTowerData.OptionValue + bonusValue;
    public string FormatingString;
    // Deep Copy 된 대상에서 사용하기
    public virtual void Init(TowerManager towerManager, TowerTable.Data baseTowerData , RandomOptionData.Data optionData)
    {
        this.optionData = optionData;
        this.towerManager = towerManager;
        this.planet = GameObject.FindWithTag("Player")?.GetComponent<BasePlanet>();
        this.baseTowerData = baseTowerData;
    }

    public void Init(TowerTable.Data baseTowerData , RandomOptionData.Data optionData)
    {
        this.baseTowerData = baseTowerData;
        this.optionData = optionData;
        SetApplyRangeFormatingString();
    }

    public RandomOptionBase DeepCopy()
    {
        RandomOptionBase copyRandomOption = CreateInstance();
        return copyRandomOption;
    }

    public void SetApplyRangeFormatingString()
    {
        int side = baseTowerData.Option_type;

        if (side == 0)
            FormatingString = $"양 옆타워 {baseTowerData.Option_Range}칸";
        else if (side == 1)
            FormatingString = $"왼쪽 {baseTowerData.Option_Range}칸";
        else if (side == 2)
            FormatingString = $"오른쪽 {baseTowerData.Option_Range}칸";
    }

    protected void GetApplyOptionTowers()
    {
        int side = baseTowerData.Option_type;

        SetApplyRangeFormatingString();
        if (side == 0)
            towers = towerManager.GetAroundTower(baseTowerData, baseTowerData.Option_Range);
        else if (side == 1)
            towers = towerManager.GetLeftTower(baseTowerData, baseTowerData.Option_Range);
        else if (side == 2)
            towers = towerManager.GetLeftTower(baseTowerData, baseTowerData.Option_Range);
    }

    public void AddBonusOptionValue(int amount)
    {
        level++;
        bonusValue += amount;
    }

    public int GetOptionValue()
    {
        return bonusValue;
    }

    public RandomOptionData.Data GetOptionData()
    {
        return optionData;
    }

    protected abstract RandomOptionBase CreateInstance();
    public abstract void ResetRandomOption();
    public abstract void SetRandomOption();
    public abstract string GetOptionStringFormatting();
}