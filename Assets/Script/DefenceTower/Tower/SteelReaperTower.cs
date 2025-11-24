using UnityEngine;

public class SteelReaperTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        SniperBullet sniperBullet = Managers.ObjectPoolManager.SpawnObject<SniperBullet>(PoolsId.SniperBullet);
        return sniperBullet;
    }
}