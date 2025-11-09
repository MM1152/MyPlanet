using Unity.VisualScripting;
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
    protected TowerData.Data data;
    protected GameObject attackprefab;
    protected bool init = false;

    public virtual void Init(GameObject tower , TowerManager manager, TowerData.Data data)
    {
        this.manager = manager;
        this.data = data;
        this.tower = tower;
    }

    public void Update(float deltaTime)
    {
        currentAttackInterval += deltaTime;

        if(target != null && targetDamageAble.IsDead)
        {
            target = null;
        }

        if(currentAttackInterval > data.attackInterval)
        {
            attackAble = true;
        }
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

            if (Vector3.Distance(target.position, tower.transform.position) > data.attackRadius)
                return false;

            attackAble = false;
            currentAttackInterval = 0;
            Debug.Log($"Attack Tower {data.name}");
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
}
