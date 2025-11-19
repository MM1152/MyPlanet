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
    private float durationTimeTimer;
    private float coolTime;
    private float coolTimeTimer;

    private float effectCycleTime;

    private bool isPassiveOn = false;
    public void Init(int passiveId)
    {
        passiveData = DataTableManager.PassiveTable.GetData(passiveId);
        effectData = DataTableManager.EffectTable.Get(passiveData.Effect_Id);
        
        this.passive = passiveFactory.CreateInstance(passiveId);
        this.condition = conditionFactory.CreateInstance(passiveData.Condition);
        this.passive.Init(passiveData , effectData);
        this.condition.Init(passiveData , effectData);

        durationTime = passiveData.Time;
        coolTime = passiveData.Cool_Time;

        durationTimeTimer = 0f;
        coolTimeTimer = 0f;
    }

    public void CheckUseAblePassive(Tower tower , BasePlanet basePlanet , Enemy enemy)
    {
        if(passiveData.Cool_Time == 0)
        {
            if(condition.CheckCondition(tower, basePlanet, enemy))
                passive.ApplyPassive(tower, basePlanet, enemy);
        }
        else
        {
            if ((coolTimeTimer >= coolTime && condition.CheckCondition(tower, basePlanet, enemy)))
            {
                if (effectCycleTime <= 0f)
                {
                    effectCycleTime = effectData.Effect_Cycle;
                    passive.ApplyPassive(tower, basePlanet, enemy);
                }
            }
        }

    }

    public void Update(float deltaTime)
    {
        if(isPassiveOn)
        {
            durationTimeTimer += deltaTime;
            effectCycleTime -= deltaTime;

            if(durationTime <= durationTimeTimer)
            {
                durationTimeTimer = 0f;
                effectCycleTime = 0f;
                coolTime = 0f;
                Debug.Log("Passive 끝남!");
            }
        }
        else
        {
            coolTimeTimer += Time.deltaTime;
        }
    }
}