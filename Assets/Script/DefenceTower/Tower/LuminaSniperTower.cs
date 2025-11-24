using UnityEngine;

public class LuminaSniperTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        return Managers.ObjectPoolManager.SpawnObject<LuminaSniperBullet>(PoolsId.LuminaSniperBullet);
    }
}
