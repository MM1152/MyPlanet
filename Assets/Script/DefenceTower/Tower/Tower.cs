using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Tower
{
    public int FullDamage => towerData.ATK + bonusDamage;
    public int BaseDamage => towerData.ATK;
    public int BonusDamage => bonusDamage;

    public float FullAttackSpeed => towerData.Fire_Rate + bonusAttackSpeed;
    public float BaseAttackSpeed => towerData.Fire_Rate;
    public float BonusAttackSpeed => bonusAttackSpeed;

    public int ID => towerData.ID;

    public int AttackRange => towerData.Range;

    public TowerTable.Data TowerData => towerData;

    protected GameObject projectTile;
    protected float attackInterval = 2f;
    protected float currentAttackInterval;

    protected GameObject tower;
    protected Transform Target
    {
        set
        {
            target = value;

            if(target != null)
            {
                targetDamageAble = target.GetComponent<IDamageAble>();
            }
            else
            {
                targetDamageAble = null;
            }
        }
    }
    protected Transform target;
    protected IDamageAble targetDamageAble;
    protected bool attackAble;

    protected TowerManager manager;
    protected TowerTable.Data towerData;
    
    protected float minNoise = 0f;
    protected float maxNoise = 0f;

    public int bonusDamage = 0;

    protected float bonusAttackSpeed = 0f;

    protected TypeEffectiveness typeEffectiveness = new TypeEffectiveness();
    private bool useAble = false;
    public bool UseAble => useAble;

    private RandomOptionData randomOptionData = new RandomOptionData();
    public RandomOptionData RandomOptionData => randomOptionData;

    protected RandomOptionData.Data optionData;
    protected RandomOptionBase baseRandomOption;
    public RandomOptionBase Option => baseRandomOption;
    
    protected string attackPrefabPath;
    protected IStatusEffect statusEffect;
    protected int slotIndex = -1;
    public int SlotIndex => slotIndex;
    public virtual void Init(GameObject tower , TowerManager manager, TowerTable.Data data , int slotIndex)
    {
        statusEffect = null;
        this.manager = manager;
        this.towerData = data;
        this.tower = tower;
        this.slotIndex = slotIndex;
        typeEffectiveness.Init((ElementType)this.towerData.Attribute);
        SetRandomOption();
        Debug.Log("Tower Inital");
    }

    private void SetRandomOption()
    {
        optionData = randomOptionData.GetData(towerData.Option);
        baseRandomOption = randomOptionData.GetRandomOptionBase(towerData.Option);
        baseRandomOption.Init(manager , towerData, optionData);
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
        
        if(target != null && !targetDamageAble.IsDead)
        {
            Target = null;
        }

        if(currentAttackInterval > 60f / FullAttackSpeed)
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

            if (Vector3.Distance(target.position, tower.transform.position) > towerData.Range)
            {
                Target = null;
                return false;
            }

            attackAble = false;
            currentAttackInterval = 0;

            Debug.Log($"Tower Attack ");

            BaseAttackPrefab attackPrefabs = CreateAttackPrefab();
            attackPrefabs.transform.position = tower.transform.position;
            attackPrefabs.Init(this, typeEffectiveness, statusEffect?.DeepCopy());
            attackPrefabs.SetTarget(target , minNoise , maxNoise);
            return true;
        }

        return false;
    }

    public virtual void LevelUp()
    {
        Debug.Log("Level Up");
    }

    public void AddBonusDamage(int damage)
    {
        bonusDamage += damage;
    }
    
    /// <summary>
    /// 보너스 스피드값 설정
    /// </summary>
    /// <param name="speed"> 0 ~ 1 사이의 값 설정 </param>
    public void AddBonusAttackSpeed(float speed)
    {
        bonusAttackSpeed += speed;
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
