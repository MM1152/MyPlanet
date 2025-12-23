using UnityEngine.AddressableAssets;

public class AbyssCoreTower : UtilTower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var gravityWrap = Managers.ObjectPoolManager.SpawnObject<GravityWrap>(PoolsId.GravityWrap);
        gravityWrap.Init(this);
        gravityWrap.Setting(tower.transform, TagIds.EnemyProjectileTag, 0, isDeleteProjectile : true);
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