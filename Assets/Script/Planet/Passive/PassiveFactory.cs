using System.Collections.Generic;
using UnityEngine;
using System;
public class PassiveFacotry : BaseFactory<IPassive>
{
    private Dictionary<int, IPassive> passiveTable = new Dictionary<int, IPassive>()
    {
        { 10001 , new EarthPassive() },
        { 10002 , new MarsPassive() },
        { 10003 , new VenusPassive() },
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
        if(basePlanet != null)
        {
            basePlanet.RepairHp(passiveData.Val);
            Debug.Log("지구 패시브 발동");
        }
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
            Debug.Log("금성 패시브 발동");
        }
    }

    public IPassive CreateInstance()
    {
        return new MarsPassive();
    }
}

public class VenusPassive : IPassive
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
        if (basePlanet != null)
        {
            basePlanet.AddBonusDEF(passiveData.Val , passiveData.Time);
            Debug.Log("비누스 패시브 발동");
        }
    }

    public IPassive CreateInstance()
    {
        return new VenusPassive();
    }
}