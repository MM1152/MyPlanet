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
        sniperBullet.SetParticleId(PoolsId.Hit10bluelaser);
        Managers.SoundManager.PlaySFX(AudiosId.sci_fi_weapon_laser_small_02);
        if(target != null)
        {
            var dir = (target.position - TowerGameObject.transform.position).normalized;
            
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash10bluelaser);
            flash.transform.position = TowerGameObject.transform.position + dir * TowerGameObject.transform.localScale.x;
        }

        return sniperBullet;
    }
}