public class MagmaBoomerTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var bullet = Managers.ObjectPoolManager.SpawnObject<MagmaBoomBullet>(PoolsId.MagmaBoomBullet);

        if(target != null)
        {
            var dir = (target.position - TowerGameObject.transform.position).normalized;
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash18novaorange);

            flash.transform.position = TowerGameObject.transform.position + dir * TowerGameObject.transform.localScale.x;
        }

        return bullet;
    }

    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }
}