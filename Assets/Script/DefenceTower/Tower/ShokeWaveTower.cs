using UnityEngine;

public class ShockWaveTower : Tower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
        statusEffect = new StunStatusEffect(0.5f);
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        return Managers.ObjectPoolManager.SpawnObject<ShockWaveBullet>(PoolsId.ShockWaveBullet);
    }
}