using JetBrains.Annotations;
using UnityEngine;

public class FrostBoomberTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        return base.Attack(false);
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
        var projectile = Managers.ObjectPoolManager.SpawnObject<MagmaBoomFregment>(PoolsId.MagmaBoomFregment);
        return projectile;
    }
}