using System;
using UnityEngine;

public class PassiveSystem
{
    private IPassive passive;
    private ICondition condition; 

    private ConditionFactory conditionFactory = new ConditionFactory();
    private PassiveFacotry passiveFactory = new PassiveFacotry();

    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    private float durationTime;
    public float durationTimeTimer;
    private float coolTime;
    private float coolTimeTimer;
    public float effectCycleTime;

    public bool isPassiveOn = false;

    private Tower tower;
    private BasePlanet basePlanet;
    private Enemy enemy;
    public void Init(int passiveId)
    {
        passiveData = DataTableManager.PassiveTable.GetData(passiveId);
        effectData = DataTableManager.EffectTable.Get(passiveData.Effect_Id);
        
        this.passive = passiveFactory.CreateInstance(passiveId);
        this.condition = conditionFactory.CreateInstance(passiveData.Condition);
        this.passive.Init(passiveData , effectData , this);
        this.condition.Init(passiveData , effectData);

        durationTime = passiveData.Time;
        coolTime = passiveData.Cool_Time;

        durationTimeTimer = 0f;
        coolTimeTimer = 0f;

        basePlanet = GameObject.FindWithTag(TagIds.PlayerTag).GetComponent<BasePlanet>();
    }

    public void CheckUseAblePassive(Tower tower , BasePlanet basePlanet , Enemy enemy)
    {
        if(passiveData.Cool_Time == 0)
        {
            if (condition.CheckCondition(tower, basePlanet, enemy))
            {
                isPassiveOn = true;
                SettingPassive(tower, basePlanet, enemy);
            }
        }
        else
        {
            if ((coolTimeTimer >= coolTime && condition.CheckCondition(tower, basePlanet, enemy)))
            {
                isPassiveOn = true;
                coolTimeTimer = 0;
                effectCycleTime = 0;
                SettingPassive(tower, basePlanet, enemy);
            }
        }
    }

    public void Update(float deltaTime)
    {
        coolTimeTimer += deltaTime;

        if(isPassiveOn)
        {
            durationTimeTimer += deltaTime;
            effectCycleTime -= deltaTime;

            if(effectCycleTime <= 0)
            {
                effectCycleTime = effectData.Effect_Cycle;
                passive.ApplyPassive(tower, basePlanet, enemy);
            }

            if (durationTimeTimer >= durationTime)
            {
                isPassiveOn = false;
                durationTimeTimer = 0f;
            }
        }
    }

    public void SettingPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        this.tower = tower;
        this.enemy = enemy;
    }
}