using UnityEngine;

public class ShadowBusterTower : Tower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }


    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var bullet = Managers.ObjectPoolManager.SpawnObject<ShadowBursterBullet>(PoolsId.ShadowBursterBullet);
        return bullet;
    }
}