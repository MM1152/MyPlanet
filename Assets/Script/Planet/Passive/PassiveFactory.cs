using System.Collections.Generic;
using UnityEngine;

public class PassiveFacotry : BaseFactory<IPassive>
{
    private Dictionary<int, IPassive> passiveTable = new Dictionary<int, IPassive>()
    {
        { 10001 , new EarthPassive() },
        { 10002 , new MarsPassive() },
    };
    public override IPassive CreateInstance(int id)
    {
        return passiveTable[id].CreateInstance();
    }
}

public class EarthPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        basePlanet.RepairHp(passiveData.Val);
        Debug.Log("지구 패시브 발동");
    }

    public IPassive CreateInstance()
    {
        return new EarthPassive();
    }
}

public class MarsPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if(enemy != null)
        {
            enemy.StatusEffect.Apply(new BurnStatusEffect(passiveData.Time , effectData.Effect_Cycle , Mathf.Abs(passiveData.Val)) , enemy);
        }
    }

    public IPassive CreateInstance()
    {
        return new MarsPassive();
    }
}