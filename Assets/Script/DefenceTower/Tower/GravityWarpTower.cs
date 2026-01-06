using UnityEngine;

public class GravityWarpTower : Tower, IFieldTower
{
    private GravityWrap currentGravityWrap;

    public override bool Attack(bool useTarget = true)
    {
        if (currentGravityWrap != null && currentGravityWrap.gameObject.activeSelf)
        {
            return false;
        }
        return base.Attack(false);
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
        currentGravityWrap = null;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    public void ResetAttackCooldown()
    {
        currentAttackInterval = 0f;
        attackAble = false;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        GravityWrap gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);
        currentGravityWrap = gravityWrap;
        gravityWrap.SetOwnerTower(this);

        return gravityWrap;
    }
}
