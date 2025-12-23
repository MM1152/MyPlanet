using UnityEngine;

public class IceRangerTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }
    
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var missile = Managers.ObjectPoolManager.SpawnObject<IceRangerMissile>(PoolsId.IceRangerMissile);
        missile.SetHitSound(AudiosId.Hit_8);
        return missile;
    }
}