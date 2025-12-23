using UnityEngine;

public class BlackMineTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var bullet = Managers.ObjectPoolManager.SpawnObject<BlackMineBullet>(PoolsId.BlackMineBullet);
        bullet.SetHitSound(AudiosId.Hit_7);
        return bullet;
    }
}