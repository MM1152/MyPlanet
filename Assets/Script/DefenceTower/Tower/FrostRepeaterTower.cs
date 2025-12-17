using UnityEngine;

public class FrostRepeaterTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }

    public override void LevelUp(LevelUpTable.Data levelUpData)
    {
        base.LevelUp(levelUpData);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet projectile = Managers.ObjectPoolManager.SpawnObject<FrostRepeaterBullet>(PoolsId.FrostRepeaterBullet);
        projectile.SetHitParticle(PoolsId.Hit6bluefire);
        var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash6bluefire);
        if(target != null)
        {
            var dir = (target.transform.position - TowerGameObject.transform.position).normalized;
            flash.transform.position = TowerGameObject.transform.position + dir * TowerGameObject.transform.localScale.x;

        }
        return projectile;
    }
}
