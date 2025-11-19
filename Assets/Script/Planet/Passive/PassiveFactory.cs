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
        { 10004 , new MercuryPassive() },
        { 10005 , new TerraPassive() },
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
            basePlanet.AddBonusDFSPercent(passiveData.Val * 0.01f , passiveData.Time);
            Debug.Log("비누스 패시브 발동");
        }
    }

    public IPassive CreateInstance()
    {
        return new VenusPassive();
    }
}

public class MercuryPassive : IPassive
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
        if (enemy != null && basePlanet != null)
        {
            float percent = enemy.TypeEffectiveness.GetDamagePercent(basePlanet.ElementType);
            int reflectionDamage = (int)(enemy.atk * percent * (passiveData.Val * 0.01f));
            enemy.OnDamage(reflectionDamage);
            Debug.Log("머큐리 패시브 발동");
        }
    }

    public IPassive CreateInstance()
    {
        return new MercuryPassive();
    }
}

public class TerraPassive : IPassive
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
        if (enemy != null && basePlanet != null)
        {
            basePlanet.AddBonusDEF(passiveData.Val, passiveData.Time);
            Debug.Log("테라 패시브 발동");
        }
    }

    public IPassive CreateInstance()
    {
        return new TerraPassive();
    }
}