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
        Managers.SoundManager.PlaySFX(AudiosId.sci_fi_weapon_laser_small_02);
        return Managers.ObjectPoolManager.SpawnObject<LuminaSniperBullet>(PoolsId.LuminaSniperBullet);
    }
}
