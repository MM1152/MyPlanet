using UnityEngine;

public class GravityWarpTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        return base.Attack(false);
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        GravityWrap gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);

        return gravityWrap;
    }
}
