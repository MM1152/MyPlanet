using UnityEngine.AddressableAssets;

public class GravityFieldTower : UtilTower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);
        gravityWrap.Init(this);
        gravityWrap.Setting(planet, TagIds.EnemyTag , BonusSlowPercent);
        gravityWrap.SetAssets(0);
        Managers.SoundManager.PlaySFX(AudiosId.SFX_Spell01Cast01);
        return gravityWrap;
    }

    public override bool Attack(bool useTarget = true)
    {
        CreateAttackPrefab();
        return true;
    }
}