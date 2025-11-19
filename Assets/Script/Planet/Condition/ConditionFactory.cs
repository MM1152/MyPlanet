using System.Collections.Generic;
using Unity.VisualScripting;

public class ConditionFactory : BaseFactory<ICondition>
{
    private Dictionary<int, ICondition> conditionTable = new Dictionary<int, ICondition>()
    {
        {1, new Under50PercentCondition() },
        {2, new Under25PercentCondition() },
        {3, new AttackFireElemetCondition() },
        {11, new HitLightElementCondition() },
        {13, new ReflectionCondition() },
        {12, new HitDarkElementCondition() },
    };

    public override ICondition CreateInstance(int id)
    {
        return conditionTable[id].CreateInstance();
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
        if (enemy == null) return false;

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