using Unity.VisualScripting;
using UnityEngine;

public abstract class Tower
{
    [SerializeField] protected GameObject projectTile;
    [SerializeField] protected float attackInterval = 2f;
    [SerializeField] protected float currentAttackInterval;
    [SerializeField] protected Transform target;
    protected IDamageAble targetDamageAble;

    private bool attackAble;
    protected TowerManager manager;
    protected TowerData.Data data;

    public virtual void Init(TowerManager manager, TowerData.Data data)
    {
        this.manager = manager;
        this.data = data;
    }

    public void Update(float deltaTime)
    {
        currentAttackInterval += deltaTime;

        if(target != null && targetDamageAble.IsDead)
        {
            target = null;
        }

        if(currentAttackInterval > attackInterval)
        {
            attackAble = true;
        }
    }

    public virtual bool Attack()
    {
        if (attackAble)
        {
            target = manager.FindTarget();
            if (target == null) 
                return false;   

            targetDamageAble = target.GetComponent<IDamageAble>();
            attackAble = false;
            currentAttackInterval = 0;
            Debug.Log($"Attack Tower {data.name}");
            return true;
        }

        return false;
    }

    public abstract void Release();
}
