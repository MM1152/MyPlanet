using System.Collections.Generic;
using UnityEngine;

public abstract class RandomOptionBase
{
    protected BasePlanet planet;
    protected TowerManager towerManager;
    protected TowerTable.Data baseTowerData;
    protected RandomOptionData.Data optionData;
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

    protected abstract RandomOptionBase CreateInstance();
    public abstract void ResetRandomOption();
    public abstract void SetRandomOption();
    public abstract string GetOptionStringFormatting();
}