using UnityEngine.AddressableAssets;

public class GravityFieldTower : UtilTower, IFieldTower
{
    private GravityWrap currentGravityWrap;

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);
        gravityWrap.Init(this);
        gravityWrap.Setting(planet, TagIds.EnemyTag , BonusSlowPercent);
        gravityWrap.SetAssets(0);
        gravityWrap.SetOwnerTower(this);
        currentGravityWrap = gravityWrap;
        Managers.SoundManager.PlaySFX(AudiosId.SFX_Spell01Cast01);
        return gravityWrap;
    }

    public override bool Attack(bool useTarget = true)
    {
        if (currentGravityWrap != null && currentGravityWrap.gameObject.activeSelf)
        {
            return false;
        }
        CreateAttackPrefab();
        return true;
    }

    public void ResetAttackCooldown()
    {
        timer = 0f;
    }
}