using UnityEngine.AddressableAssets;

public class DelayFieldTower : UtilTower, IFieldTower
{
    private GravityWrap currentGravityWrap;

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);
        gravityWrap.Init(this);
        gravityWrap.Setting(planet, TagIds.EnemyProjectileTag, BonusSlowBulletSpeed);
        gravityWrap.SetAssets(3);
        gravityWrap.SetOwnerTower(this);
        currentGravityWrap = gravityWrap;
        Managers.SoundManager.PlaySFX(AudiosId.SFX_Spell01Dark);
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