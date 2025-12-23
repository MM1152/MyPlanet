using UnityEngine.AddressableAssets;

public class DelayFieldTower : UtilTower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);
        gravityWrap.Init(this);
        gravityWrap.Setting(planet, TagIds.EnemyProjectileTag, BonusSlowBulletSpeed);
        gravityWrap.SetAssets(1);
        Managers.SoundManager.PlaySFX(AudiosId.SFX_Spell01Dark);
        return gravityWrap;
    }

    public override bool Attack(bool useTarget = true)
    {
        CreateAttackPrefab();
        return true;
    }
}