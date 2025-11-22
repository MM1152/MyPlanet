using UnityEngine;

public class IceRangerTower : Tower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var missile = Managers.ObjectPoolManager.SpawnObject<IceRangerMissile>(PoolsId.IceRangerMissile);
        missile.Init(this);
        return missile;
    }
}