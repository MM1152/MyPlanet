using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Tower
{
    public int Damage => towerData.ATK + bonusDamage;
    public int ID => towerData.ID;


    protected GameObject projectTile;
    protected float attackInterval = 2f;
    protected float currentAttackInterval;

    protected GameObject tower;
    protected Transform target;
    protected IDamageAble targetDamageAble;
    protected bool attackAble;

    protected TowerManager manager;
    protected TowerTable.Data towerData;
    public TowerTable.Data TowerData => towerData;
    protected GameObject attackprefab;
    protected bool init = false;
    
    // 해당 방향으로 날라갈때의 노이즈 값
    // 해당 값을 통해서 탄퍼짐 구성 예정
    protected float minNoise = 0f;
    protected float maxNoise = 0f;

    public int bonusDamage = 0;

    protected float bonusAttackSpeed = 1f;

    protected TypeEffectiveness typeEffectiveness = new TypeEffectiveness();
    private bool useAble = false;
    public bool UseAble => useAble;
    //Fix: 테스트 용임
    private RandomOptionData randomOptionData = new RandomOptionData();
    public RandomOptionData RandomOptionData => randomOptionData;

    protected RandomOptionData.Data optionData;
    protected RandomOptionBase baseRandomOption;
    public RandomOptionBase Option => baseRandomOption;
    
    protected string attackPrefabPath;
    protected IStatusEffect statusEffect;

    public virtual void Init(GameObject tower , TowerManager manager, TowerTable.Data data)
    {
        statusEffect = null;
        this.manager = manager;
        this.towerData = data;
        this.tower = tower;
        typeEffectiveness.Init((ElementType)this.towerData.Attribute);
        SetRandomOption();
        Debug.Log("Tower Inital");
    }

    public void SetLoadAttackPrefab(string path)
    {
        LoadProjectTileAsync(path).Forget();
    }

    private void SetRandomOption()
    {
        //Option Value Change 되면 맞춰서 Update 해줘야됌
        if(towerData.Option == 0)
        {
            optionData = randomOptionData.GetRandomOption();
            baseRandomOption = randomOptionData.GetRandomOptionBase(optionData.id);
            baseRandomOption.Init(manager , towerData, optionData);
            towerData.Option = optionData.id;
        }
        else
        {
            optionData = randomOptionData.GetData(towerData.Option);
            baseRandomOption = randomOptionData.GetRandomOptionBase(optionData.id);
            baseRandomOption.Init(manager , towerData, optionData);
        }
    }

    public void ResetRandomOption()
    {
        towerData.Option = -1;
        SetRandomOption();
    }

    public virtual void Update(float deltaTime)
    {
        if (!useAble)
            return;

        currentAttackInterval += deltaTime * bonusAttackSpeed;

        if(target != null && !targetDamageAble.IsDead)
        {
            target = null;
        }

        if(currentAttackInterval > towerData.Fire_Rate)
        {
            attackAble = true;
            Attack();
        }
    }

    private async UniTaskVoid LoadProjectTileAsync(string path)
    {
        attackprefab = await Addressables.LoadAssetAsync<GameObject>(path).ToUniTask();
        init = true;
    }

    public virtual bool Attack()
    {
        if (!init) return false;

        if (attackAble)
        {
            if (target == null) 
                return false;
            targetDamageAble = target.GetComponent<IDamageAble>();

            if (Vector3.Distance(target.position, tower.transform.position) > towerData.Range)
            {
                target = null;
                return false;
            }

            attackAble = false;
            currentAttackInterval = 0;

            BaseAttackPrefab attackPrefabs = CreateAttackPrefab();
            attackPrefabs.transform.position = tower.transform.position;
            attackPrefabs.Init(this, typeEffectiveness, statusEffect?.DeepCopy());
            attackPrefabs.SetTarget(target , minNoise , maxNoise);
            return true;
        }

        return false;
    }

    public virtual void Release()
    {
        if(attackprefab != null)
        {
            Addressables.Release(attackprefab);
        }
    }

    public virtual void LevelUp()
    {
        Debug.Log("Level Up");
    }

    public void AddBonusDamage(int damage)
    {
        bonusDamage += damage;
    }

    public void AddBonusAttackSpeed(float speed)
    {
        bonusAttackSpeed += speed;
    }

    public void PlaceTower()
    {
        useAble = true;
        baseRandomOption.SetRandomOption();
    }

    protected abstract BaseAttackPrefab CreateAttackPrefab();
}
