using UnityEngine.AddressableAssets;

public class DelayFieldTower : UtilTower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);
        gravityWrap.Init(this);
        gravityWrap.Setting(planet, TagIds.EnemyProjectileTag, BonusSlowBulletSpeed);
        return gravityWrap;
    }

    public override bool Attack(bool useTarget = true)
    {
        CreateAttackPrefab();
        return true;
    }
}