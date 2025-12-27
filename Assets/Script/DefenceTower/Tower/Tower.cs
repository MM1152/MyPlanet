using UnityEngine;

public abstract class Tower
{
    public int FullDamage => towerData.ATK + BonusDamage ;
    public int BaseDamage => towerData.ATK;   
    

    public float FullAttackSpeed => (towerData.Fire_Rate + BonusAttackSpeed + BonusFireRate) * (1 + BonusAttackSpeedPercent);
    public float BaseAttackSpeed => towerData.Fire_Rate;
    public float FullAttackRange => towerData.Attack_Range + BonusAttackRange;
    public float BaseAttackRange => towerData.Attack_Range;

    public float FullNoise => noise + BonuseNoise;

    public int CalcurateAttackDamage => planetData != null ? (int)((FullDamage + planetData.ATK * 0.1f) * (1 + BonusDamagePercent)) : FullDamage;
    
    public float BonusAttackSpeedPercent { get; set; }
    public float BonusDamagePercent { get; set; }
    public float BonusAttackSpeed { get; set; }
    public int BonusDamage { get; set; }
    public int BonusProjectileCount { get; set; }
    public int BonusAttackRange { get; set; }
    public int BonusWidthSize { get; set; }
    public float BonusDuration { get; set; }
    public float BonusCoolTime { get; set; }
    public int BonusFireRate { get; set; }
    public int BonusPelletCount { get; set; }
    public int BonusFregmentRange { get; set; }
    public int BonusFregmentCount { get; set; }
    public int BonusExplosionRange { get; set; }
    public int BonusTargetingCount { get; set; }
    public float BonusSlowPercent { get; set; }
    public float BonusSlowBulletSpeed { get; set; }
    public float BonusStopTime { get; set; }
    public int BonuseNoise { get; set; }
    public int BonusBulletSpeed { get; set; }
    public int BonusDroneCount { get; set; }
    public int BonusDroneHp { get; set; }
    public int BonusDroneTargetedPercent { get; set; }

    public float AttackRange => towerData.Attack_Range;
    public int SlotIndex => slotIndex;
    public int ID => towerData.ID;
    public bool UseAble => useAble;
    public int Level => level;

    public GameObject TowerGameObject => tower;
    public TowerTable.Data TowerData => towerData;
    public TowerManager towerManager => manager;
    public Transform Target
    {
        protected set
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

        get => target;
    }
    public TypeEffectiveness TypeEffectiveness => typeEffectiveness;
    public IStatusEffect StatusEffect => statusEffect;
    public RandomOptionBase Option => baseRandomOption;

    private PlanetLevelUpTable.Data planetData;
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
    private LevelUpTable.Data levelUpData;

    protected RandomOptionData.Data optionData;
    protected RandomOptionBase baseRandomOption;

    protected int level = 0;
    protected string attackPrefabPath;
    protected IStatusEffect statusEffect;
    protected int slotIndex = -1;

    public bool IsHelper { get; set; } = false;

    //Debug 용임 지우면 X
    public void SetPlanetData(PlanetTable.Data planetData)
    {
        var planetId = planetData.ID;
        var userData = FirebaseManager.Instance.PlanetData.GetOrigin(planetId);
        this.planetData = userData.PlanetLevelData;
    }

    //Helper 용 타워 설치
    public void Init(int fulldamage , GameObject tower , TowerManager towerManager , TowerTable.Data data)
    {
        this.manager = towerManager;
        this.towerData = data;
        this.tower = tower;

        try
        {
            BonusDamage = fulldamage;
        }
        finally
        {
            typeEffectiveness.Init((ElementType)this.towerData.attribute);
        }

        IsHelper = true;
    }

    public virtual void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        statusEffect = null;
        this.manager = manager;
        this.towerData = data;
        this.tower = tower;
        this.slotIndex = slotIndex;        
        try
        {
            var gameData = FirebaseManager.Instance.PresetData.GetGameData().data;
            if(gameData != null)
            {
                var planetId = gameData.PlanetId;
                var userData = FirebaseManager.Instance.PlanetData.GetOrigin(planetId);
                this.planetData = userData.PlanetLevelData;
            }
        }
        finally
        {
            typeEffectiveness.Init((ElementType)this.towerData.attribute);
            SetRandomOption();
        }
    }

    private void SetRandomOption()
    {
        optionData = RandomOptionData.GetData(towerData.Option);
        baseRandomOption = RandomOptionData.GetRandomOptionBase(towerData.Option);
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

    public virtual bool Attack(bool useTarget = true)
    {
        if (attackAble)
        {
            if(useTarget)
            {
                if (target == null)
                    return false;

                if (Vector3.Distance(target.position, tower.transform.position) > FullAttackRange)
                {
                    Target = null;
                    return false;
                }

                var dir = (target.position - TowerGameObject.transform.position).normalized ;
                TowerGameObject.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + -90f);
            }

            attackAble = false;
            currentAttackInterval = 0;

            BaseAttackPrefab attackPrefabs = CreateAttackPrefab();
            attackPrefabs.transform.position = tower.transform.position;
            attackPrefabs.Init(this);
            if (target != null)
                attackPrefabs.SetTarget(target , FullNoise);
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

    public virtual void LevelDown(LevelUpTable.Data levelUpData)
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

        level--;
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

    private void CheckLevelUpVariable(int variable, float value)
    {
        switch (variable)
        {
            case 1:
                BonusProjectileCount += (int)value;
                break;
            case 2:
                BonusAttackRange += (int)value;
                break;
            case 3:
                BonusWidthSize += (int)value;
                break;
            case 4:
                BonusDuration += value;
                break;
            case 5:
                BonusCoolTime += value;
                break;
            case 6:
                BonusFireRate += (int)value;
                break;
            case 7:
                BonusPelletCount += (int)value;
                break;
            case 8:
                BonusFregmentRange += (int)value;
                break;
            case 9:
                BonusFregmentCount += (int)value;
                break;
            case 10:
                BonusExplosionRange += (int)value;
                break;
            case 11:
                BonusTargetingCount += (int)value;
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
                BonuseNoise += (int)value;
                break;
            case 16:
                BonusBulletSpeed += (int)value;
                break;
            case 17:
                BonusDroneCount += (int)value;
                break;
            case 18:
                BonusDroneHp += (int)value;
                break;
            case 19:
                BonusDroneTargetedPercent += (int)value;
                break;
        }
    }
    
    public void AddBonusDamage(int damage)
    {
        BonusDamage += damage;
    }

    public void AddBonusDamageToPercent(float percent)
    {
        BonusDamagePercent += percent;
    }

    public void MinusBonusDamageToPercent(float percent)
    {
        BonusDamagePercent -= percent;
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
        BonusAttackSpeedPercent += percent;
    }

    public void MinusBonusAttackSpeedTopercent(float percent)
    {
        BonusAttackSpeedPercent -= percent;
    }

    public virtual void PlaceTower(bool isHelper = false)
    {
        useAble = true;
        if(!IsHelper)
           baseRandomOption.SetRandomOption();
    }

    public virtual void UnPlaceTower()
    {
        useAble = false;
        baseRandomOption.ResetRandomOption();
    }

    public ElementType GetElementType()
    {
        return typeEffectiveness.Type;
    }

    public void SetStatusEffect(IStatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
    }
   
    // 명왕성 용
    public ElementType GetPlanetElement()
    {
        return (ElementType)DataTableManager.PlanetTable.Get(planetData.ID).Attribute;
    }
    protected abstract BaseAttackPrefab CreateAttackPrefab();
}
