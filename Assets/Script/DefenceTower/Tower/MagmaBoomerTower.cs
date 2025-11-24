public class MagmaBoomerTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var bullet = Managers.ObjectPoolManager.SpawnObject<MagmaBoomBullet>(PoolsId.MagmaBoomBullet);
        return bullet;
    }

    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }
}