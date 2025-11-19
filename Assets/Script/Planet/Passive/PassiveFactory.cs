using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
public class PassiveFacotry : BaseFactory<IPassive>
{
    private Dictionary<int, IPassive> passiveTable = new Dictionary<int, IPassive>()
    {
        { 10001 , new EarthPassive() },
        { 10002 , new MarsPassive() },
        { 10003 , new VenusPassive() },
        { 10004 , new MercuryPassive() },
        { 10005 , new TerraPassive() },
        { 10006 , new JupiterPassive() },
        { 10007 , new SaturnPassive() },
        { 10008 , new UranusPassive() },
        { 10009 , new NeptunePassive() },
        { 10013 , new ErisPassive() },
        {10014 , new CeresPassive() },
        {10015 , new LumicillaPassive() }
    };
    public override IPassive CreateInstance(int id)
    {
        return passiveTable[id].CreateInstance();
    }
}

public class EarthPassive :  IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData , PassiveSystem passiveSystem)
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

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData , PassiveSystem passiveSystem)
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
    private PassiveSystem passiveSystem;
    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData , PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
    }

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if (basePlanet != null)
        {
            basePlanet.AddBonusDFSPercent(passiveData.Val * 0.01f , passiveData.Time);
            passiveSystem.isPassiveOn = false;
            passiveSystem.durationTimeTimer = 0f;
            passiveSystem.effectCycleTime = 0f;
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
    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData , PassiveSystem passiveSystem)
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
    private PassiveSystem passiveSystem;
    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
    }

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if (basePlanet != null)
        {
            basePlanet.AddBonusDEF(passiveData.Val, passiveData.Time);
            passiveSystem.isPassiveOn = false;
            passiveSystem.durationTimeTimer = 0f;
            passiveSystem.effectCycleTime = 0f;

            Debug.Log("테라 패시브 발동");
        }
    }

    public IPassive CreateInstance()
    {
        return new TerraPassive();
    }
}

public class JupiterPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;
    private PassiveSystem passiveSystem;
    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
    }

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if (basePlanet != null)
        {
            basePlanet.RepairHp(passiveData.Val);
            Debug.Log("목성 패시브 발동");
        }
    }

    public IPassive CreateInstance()
    {
        return new JupiterPassive();
    }
}

public class SaturnPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;
    private PassiveSystem passiveSystem;

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if ( basePlanet != null )
        {
            float rand = UnityEngine.Random.Range(0f , 1f);
            if(rand <= 0.35f)
            {
                basePlanet.AddShield(120);
                Debug.Log("토성 패시브 적용");
            }
        }
    }

    public IPassive CreateInstance()
    {
        return new SaturnPassive();
    }

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
    }
}

public class UranusPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;
    private PassiveSystem passiveSystem;

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if (enemy != null && basePlanet != null) 
        {
            enemy.StatusEffect.Apply(new SlowStatusEffect(passiveData.Time, Mathf.Abs(passiveData.Val) * 0.01f) , enemy);
            passiveSystem.durationTimeTimer = 0f;
            passiveSystem.isPassiveOn = false;
            Debug.Log("천왕성 패시브 적용");
        }
    }

    public IPassive CreateInstance()
    {
        return new UranusPassive();
    }

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
    }
}

public class NeptunePassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;
    private PassiveSystem passiveSystem;

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if (tower != null)
        {
            basePlanet.RepairHpToPercent(passiveData.Val * 0.01f);
            Debug.Log("해왕성 패시브 적용");
        }
    }

    public IPassive CreateInstance()
    {
        return new NeptunePassive();
    }

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
    }
}

public class ErisPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;
    private PassiveSystem passiveSystem;

    private float timer = 0f;

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        timer += Time.deltaTime;

        if(timer >= 1f)
        {
            timer = 0f;
            basePlanet.OnDamage(passiveData.Val);
        }

        if (enemy != null)
        {
            if(enemy.IsDead)
            {
                basePlanet.RepairHp(passiveData.Val * 2);
                Debug.Log("에리스 패시브 적용");
            }
        }
    }

    public IPassive CreateInstance()
    {
        return new ErisPassive();
    }

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
    }
}

public class CeresPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;
    private PassiveSystem passiveSystem;
    private TowerManager towerManager;

    private float timer = 0f;
    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if (tower != null)
        {
            var towers = towerManager.GetAllTower();   
            foreach(var applytower in towers)
            {
                if(applytower != null)
                    applytower.AddBonusAttackSpeedTopercent(passiveData.Val * 0.01f);
            }
            UniTask.RunOnThreadPool(() => WaitForSecondAsync(passiveData.Time)).Forget();
            passiveSystem.isPassiveOn = false;
            Debug.Log("세레스 패시브 적용");
        }
    }

    public IPassive CreateInstance()
    {
        return new CeresPassive();
    }

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
        towerManager = GameObject.FindWithTag(TagIds.TowerManagerTag).GetComponent<TowerManager>();
    }

    private async UniTask WaitForSecondAsync(float time)
    {
        await UniTask.WaitForSeconds(time);
        var towers = towerManager.GetAllTower();
        foreach (var applytower in towers)
        {
            if (applytower != null)
                applytower.MinusBonusAttackSpeedTopercent(passiveData.Val * 0.01f);
        }
        Debug.Log("세레스 패시브 해제");
    }
}

public class LumicillaPassive : IPassive
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;
    private PassiveSystem passiveSystem;
    private TowerManager towerManager;

    public void ApplyPassive(Tower tower, BasePlanet basePlanet, Enemy enemy)
    {
        if (tower != null)
        {
            var towers = towerManager.GetAllTower();
            foreach (var applytower in towers)
            {
                if (applytower != null)
                    applytower.AddBonusDamageToPercent(passiveData.Val * 0.01f);
            }
            UniTask.RunOnThreadPool(() => WaitForSecondAsync(passiveData.Time)).Forget();
            passiveSystem.isPassiveOn = false;
            Debug.Log("루미실라 패시브 적용");
        }
    }

    public IPassive CreateInstance()
    {
        return new LumicillaPassive();
    }

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData, PassiveSystem passiveSystem)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
        this.passiveSystem = passiveSystem;
        towerManager = GameObject.FindWithTag(TagIds.TowerManagerTag).GetComponent<TowerManager>();
    }

    private async UniTask WaitForSecondAsync(float time)
    {
        await UniTask.WaitForSeconds(time);
        var towers = towerManager.GetAllTower();
        foreach (var applytower in towers)
        {
            if (applytower != null)
                applytower.MinusBonusDamageToPercent(passiveData.Val * 0.01f);
        }
        Debug.Log("루미실라 패시브 해제");
    }
}