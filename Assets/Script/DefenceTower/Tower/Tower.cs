using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Tower
{
    public int FullDamage => towerData.ATK + BonusDamage;
    public int BaseDamage => towerData.ATK;

    public float FullAttackSpeed => towerData.Fire_Rate + BonusAttackSpeed;
    public float BaseAttackSpeed => towerData.Fire_Rate;

    public float FullAttackRange => towerData.Attack_Range + BonusAttackRange;
    public float BaseAttackRange => towerData.Attack_Range;

    public float FullNoise => noise + BonuseNoise;

    public float BonusAttackSpeed { get; set; }
    public int BonusDamage { get; set; }
    public int BonusProjectileCount { get; set; }
    public int BonusAttackRange { get; set; }
    public int BonusWidthSize { get; set; }
    public int BonusDuration { get; set; }
    public int BonusCoolTime { get; set; }
    public int BonusFireRate { get; set; }
    public int BonusPelletCount { get; set; }
    public int BonusFregmentRange { get; set; }
    public int BonusFregmentCount { get; set; }
    public int BonusExplosionRange { get; set; }
    public int BonusTargetingCount { get; set; }
    public int BonusSlowPercent { get; set; }
    public int BonusSlowBulletSpeed { get; set; }
    public int BonusStopTime { get; set; }
    public int BonuseNoise { get; set; }
    public int BonusBulletSpeed { get; set; }

    public float AttackRange => towerData.Attack_Range;
    public int SlotIndex => slotIndex;
    public int ID => towerData.ID;
    public bool UseAble => useAble;
    public int Level => level;

    public GameObject TowerGameObject => tower;
    public TowerTable.Data TowerData => towerData;
    public TowerManager towerManager => manager;
    protected Transform Target
    {
        set
        {
            target = value;

            if (target != null)
            {
                targetDamageAble = target.GetComponent<IDamageAble>();
            }
            else
            {
                targetDamageAble = null;
            }
        }
    }
    public TypeEffectiveness TypeEffectiveness => typeEffectiveness;
    public RandomOptionData RandomOptionData => randomOptionData;
    public IStatusEffect StatusEffect => statusEffect;
    public RandomOptionBase Option => baseRandomOption;

    protected GameObject projectTile;
    protected GameObject tower;

    protected Transform target;
    protected IDamageAble targetDamageAble;
    protected TowerManager manager;
    protected TowerTable.Data towerData;

    protected float currentAttackInterval;
    protected float bonusAttackSpeed = 0f;
    protected float noise = 0f;

    protected bool attackAble;
    private bool useAble = false;

    protected TypeEffectiveness typeEffectiveness = new TypeEffectiveness();
    private RandomOptionData randomOptionData = new RandomOptionData();
    private LevelUpTable.Data levelUpData;

    protected RandomOptionData.Data optionData;
    protected RandomOptionBase baseRandomOption;

    protected int level = 0;
    protected string attackPrefabPath;
    protected IStatusEffect statusEffect;
    protected int slotIndex = -1;

    public virtual void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        statusEffect = null;
        this.manager = manager;
        this.towerData = data;
        this.tower = tower;
        this.slotIndex = slotIndex;

        typeEffectiveness.Init((ElementType)this.towerData.Attribute);
        SetRandomOption();
    }

    private void SetRandomOption()
    {
        optionData = randomOptionData.GetData(towerData.Option);
        baseRandomOption = randomOptionData.GetRandomOptionBase(towerData.Option);
        baseRandomOption.Init(manager, towerData, optionData);
    }

    public void ResetRandomOption()
    {
        towerData.Option = -1;
        SetRandomOption();
    }

    public virtual void Update(float deltaTime)
    {
        if (!useAble) return;
        currentAttackInterval += deltaTime;

        if (target != null && !targetDamageAble.IsDead)
        {
            Target = null;
        }

        if (currentAttackInterval > 60f / FullAttackSpeed)
        {
            attackAble = true;
            Attack();
        }
    }

    public virtual bool Attack()
    {
        if (attackAble)
        {
            if (target == null)
                return false;

            if (Vector3.Distance(target.position, tower.transform.position) > FullAttackRange)
            {
                Target = null;
                return false;
            }

            attackAble = false;
            currentAttackInterval = 0;

            BaseAttackPrefab attackPrefabs = CreateAttackPrefab();
            attackPrefabs.transform.position = tower.transform.position;
            attackPrefabs.Init(this);
            attackPrefabs.SetTarget(target, FullNoise);
            return true;
        }

        return false;
    }

    public virtual void LevelUp(LevelUpTable.Data levelUpData)
    {
        var var1 = 0;
        var var2 = 0;
        var var3 = 0;
        var var4 = 0;
        if (level != 0)
        {
            BonusDamage -= this.levelUpData.Damage;
            var1 = this.levelUpData.Var1;
            var2 = this.levelUpData.Var2;
            var3 = this.levelUpData.Var3;
            var4 = this.levelUpData.Var4;
            CheckLevelUpVariable(var1, -this.levelUpData.Val1);
            CheckLevelUpVariable(var2, -this.levelUpData.Val2);
            CheckLevelUpVariable(var3, -this.levelUpData.Val3);
            CheckLevelUpVariable(var4, -this.levelUpData.Val4);
        }

        level++;
        this.levelUpData = levelUpData;
        BonusDamage += this.levelUpData.Damage;
        var1 = this.levelUpData.Var1;
        var2 = this.levelUpData.Var2;
        var3 = this.levelUpData.Var3;
        var4 = this.levelUpData.Var4;
        CheckLevelUpVariable(var1, this.levelUpData.Val1);
        CheckLevelUpVariable(var2, this.levelUpData.Val2);
        CheckLevelUpVariable(var3, this.levelUpData.Val3);
        CheckLevelUpVariable(var4, this.levelUpData.Val4);
    }

    private void CheckLevelUpVariable(int variable, int value)
    {
        switch (variable)
        {
            case 1:
                BonusProjectileCount += value;
                break;
            case 2:
                BonusAttackRange += value;
                break;
            case 3:
                BonusWidthSize += value;
                break;
            case 4:
                BonusDuration += value;
                break;
            case 5:
                BonusCoolTime += value;
                break;
            case 6:
                BonusFireRate += value;
                break;
            case 7:
                BonusPelletCount += value;
                break;
            case 8:
                BonusFregmentRange += value;
                break;
            case 9:
                BonusFregmentCount += value;
                break;
            case 10:
                BonusExplosionRange += value;
                break;
            case 11:
                BonusTargetingCount += value;
                break;
            case 12:
                BonusSlowPercent += value;
                break;
            case 13:
                BonusSlowBulletSpeed += value;
                break;
            case 14:
                BonusStopTime += value;
                break;
            case 15:
                BonuseNoise += value;
                break;
            case 16:
                BonusBulletSpeed += value;
                break;
        }
    }
    
    public void AddBonusDamage(int damage)
    {
        BonusDamage += damage;
    }

    public void AddBonusDamageToPercent(float percent)
    {
        BonusDamage += (int)(BaseDamage * percent);
    }

    public void MinusBonusDamageToPercent(float percent)
    {
        BonusDamage -= (int)(BaseDamage * percent);
    }

    /// <summary>
    /// 보너스 스피드값 설정
    /// </summary>
    /// <param name="speed"> 0 ~ 1 사이의 값 설정 </param>
    public void AddBonusAttackSpeed(float speed)
    {
        bonusAttackSpeed += speed;
    }

    public void AddBonusAttackSpeedTopercent(float percent)
    {
        bonusAttackSpeed += BaseAttackSpeed * percent;
    }

    public void MinusBonusAttackSpeedTopercent(float percent)
    {
        bonusAttackSpeed -= BaseAttackSpeed * percent;
    }

    public void PlaceTower()
    {
        useAble = true;
        baseRandomOption.SetRandomOption();
    }

    public ElementType GetElementType()
    {
        return typeEffectiveness.Type;
    }

    public void SetStatusEffect(IStatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
    }
    protected abstract BaseAttackPrefab CreateAttackPrefab();
}
