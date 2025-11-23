using UnityEngine;

public class LuminaSniperTower : Tower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        return Managers.ObjectPoolManager.SpawnObject<LuminaSniperBullet>(PoolsId.LuminaSniperBullet);
    }
}
