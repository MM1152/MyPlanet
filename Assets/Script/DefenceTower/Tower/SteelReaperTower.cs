using UnityEngine;

public class SteelReaperTower : Tower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        SniperBullet sniperBullet = Managers.ObjectPoolManager.SpawnObject<SniperBullet>(PoolsId.SniperBullet);
        return sniperBullet;
    }
}