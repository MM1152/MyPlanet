using UnityEngine;

public class FrostRepeaterTower : Tower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }

    public override void LevelUp(LevelUpTable.Data levelUpData)
    {
        base.LevelUp(levelUpData);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet projectile = Managers.ObjectPoolManager.SpawnObject<Bullet>(PoolsId.Bullet);
        return projectile;
    }
}
