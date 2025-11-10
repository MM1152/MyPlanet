using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Tower
{
    [SerializeField] protected GameObject projectTile;
    [SerializeField] protected float attackInterval = 2f;
    [SerializeField] protected float currentAttackInterval;

    protected GameObject tower;
    protected Transform target;
    protected IDamageAble targetDamageAble;
    protected bool attackAble;

    protected TowerManager manager;
    protected TowerTable.Data towerData;
    protected GameObject attackprefab;
    protected bool init = false;
    
    // 해당 방향으로 날라갈때의 노이즈 값
    // 해당 값을 통해서 탄퍼짐 구성 예정
    protected float minNoise = 0f;
    protected float maxNoise = 0f;

    protected int bonusDamage = 0;

    protected TypeEffectiveness typeEffectiveness = new TypeEffectiveness();
    private bool useAble = false;

    public virtual void Init(GameObject tower , TowerManager manager, TowerTable.Data data)
    {
        this.manager = manager;
        this.towerData = data;
        this.tower = tower;

        typeEffectiveness.Init((ElementType)this.towerData.Attribute);
        LoadProjectTileAsync().Forget();
    }

    public virtual void Update(float deltaTime)
    {
        if (!useAble)
            return;

        currentAttackInterval += deltaTime;

        if(target != null && targetDamageAble.IsDead)
        {
            target = null;
        }

        if(currentAttackInterval > towerData.Fire_Rate)
        {
            attackAble = true;
        }
    }

    private async UniTaskVoid LoadProjectTileAsync()
    {
        attackprefab = await Addressables.LoadAssetAsync<GameObject>(towerData.projectilePrefabPath).ToUniTask();
        init = true;
    }

    public virtual bool Attack()
    {
        if (!init) return false;

        if (attackAble)
        {
            target = manager.FindTarget();

            if (target == null) 
                return false;
            targetDamageAble = target.GetComponent<IDamageAble>();

            if (Vector3.Distance(target.position, tower.transform.position) > towerData.AttackRadius)
                return false;

            attackAble = false;
            currentAttackInterval = 0;

            BaseAttackPrefab attackPrefabs = CreateAttackPrefab();
            attackPrefabs.transform.position = tower.transform.position;
            attackPrefabs.Init(towerData, typeEffectiveness);
            attackPrefabs.SetTarget(target , minNoise , maxNoise);

            Debug.Log($"Attack Tower {towerData.Name}");

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

    public void PlaceTower()
    {
        useAble = true;
    }

    protected abstract BaseAttackPrefab CreateAttackPrefab();
}
