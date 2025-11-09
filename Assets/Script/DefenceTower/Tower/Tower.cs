using Cysharp.Threading.Tasks;
using System;
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
    private bool attackAble;

    protected TowerManager manager;
    protected TowerData.Data towerData;
    protected GameObject attackprefab;
    protected bool init = false;

    protected TypeEffectiveness typeEffectiveness = new TypeEffectiveness();

    public virtual void Init(GameObject tower , TowerManager manager, TowerData.Data data)
    {
        this.manager = manager;
        this.towerData = data;
        this.tower = tower;

        typeEffectiveness.Init((ElementType)this.towerData.type);
        LoadProjectTileAsync().Forget();
    }

    public void Update(float deltaTime)
    {
        currentAttackInterval += deltaTime;

        if(target != null && targetDamageAble.IsDead)
        {
            target = null;
        }

        if(currentAttackInterval > towerData.attackInterval)
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

            if (Vector3.Distance(target.position, tower.transform.position) > towerData.attackRadius)
                return false;

            attackAble = false;
            currentAttackInterval = 0;

            BaseAttackPrefab attackPrefabs = CreateAttackPrefab();
            attackPrefabs.transform.position = tower.transform.position;
            attackPrefabs.Init(towerData, typeEffectiveness);
            attackPrefabs.SetTarget(target);

            Debug.Log($"Attack Tower {towerData.name}");

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

    protected abstract BaseAttackPrefab CreateAttackPrefab();
}
