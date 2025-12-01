using System.Collections.Generic;

public class  ConditionFactory : BaseFactory<ICondition>
{
    private Dictionary<int, ICondition> conditionTable = new Dictionary<int, ICondition>()
    {
        {0, new NoCondition() },
        {1, new Under50PercentCondition() },
        {2, new Under25PercentCondition() },
        {3, new AttackFireElemetCondition() },
        {11, new HitLightElementCondition() },
        {13, new ReflectionCondition() },
        {12, new HitDarkElementCondition() },
        {9, new HitIceElementCondition() },
        {4, new AttackIceElemetCondition() },
        {7, new AttackDarkElemetCondition() },
        {6, new AttackLightElemetCondition() },
        {5, new AttackSteelElemetCondition()  },
        {14, new OnDamageCondition()  }
    };

    public override ICondition CreateInstance(int id)
    {
        return conditionTable[id].CreateInstance();
    }
}

public class NoCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        return true;
    }

    public ICondition CreateInstance()
    {
        return new NoCondition();
    }

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }
}

public class Under50PercentCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private  EffectTable.Data effectData;

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (planet == null) return false;

        return (float)planet.hp / planet.maxHp < 0.5f;
    }

    public ICondition CreateInstance()
    {
        return new Under50PercentCondition();
    }

    public void Init(PassiveTable.Data passiveData , EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }
}

public class Under25PercentCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData , EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (planet == null) return false;

        return planet.hp / planet.maxHp < 0.25f;
    }

    public ICondition CreateInstance()
    {
        return new Under50PercentCondition();
    }
}

public class AttackFireElemetCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (tower == null) return false;

        return tower.GetElementType() == ElementType.Fire;
    }

    public ICondition CreateInstance()
    {
        return new AttackFireElemetCondition();
    }
}

public class HitLightElementCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (enemy == null) return false;

        return enemy.ElementType == ElementType.Light;
    }

    public ICondition CreateInstance()
    {
        return new HitLightElementCondition();
    }
}

public class HitDarkElementCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (enemy == null || planet == null) return false;

        return enemy.ElementType == ElementType.Dark;
    }

    public ICondition CreateInstance()
    {
        return new HitDarkElementCondition();
    }
}

public class ReflectionCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (enemy == null || planet == null) return false;

        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand < passiveData.Val * 0.01f)
            return true;

        return false;
    }

    public ICondition CreateInstance()
    {
        return new ReflectionCondition();
    }
}

public class HitIceElementCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (enemy == null || planet == null) return false;

        return enemy.ElementType == ElementType.Ice;
    }

    public ICondition CreateInstance()
    {
        return new HitIceElementCondition();
    }
}

public class AttackIceElemetCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (tower == null) return false;

        return tower.GetElementType() == ElementType.Ice;
    }

    public ICondition CreateInstance()
    {
        return new AttackIceElemetCondition();
    }
}

public class AttackDarkElemetCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (tower == null) return false;

        return tower.GetElementType() == ElementType.Dark;
    }

    public ICondition CreateInstance()
    {
        return new AttackDarkElemetCondition();
    }
}

public class AttackLightElemetCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (tower == null) return false;

        return tower.GetElementType() == ElementType.Light;
    }

    public ICondition CreateInstance()
    {
        return new AttackLightElemetCondition();
    }
}

public class AttackSteelElemetCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (tower == null) return false;

        return tower.GetElementType() == ElementType.Steel;
    }

    public ICondition CreateInstance()
    {
        return new AttackSteelElemetCondition();
    }
}

public class OnDamageCondition : ICondition
{
    private PassiveTable.Data passiveData;
    private EffectTable.Data effectData;

    public void Init(PassiveTable.Data passiveData, EffectTable.Data effectData)
    {
        this.passiveData = passiveData;
        this.effectData = effectData;
    }

    public bool CheckCondition(Tower tower, BasePlanet planet, Enemy enemy)
    {
        if (tower == null || enemy == null) return false;

        return true;
    }

    public ICondition CreateInstance()
    {
        return new OnDamageCondition();
    }
}