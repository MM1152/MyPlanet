using UnityEngine;
using System;

public class VolcanoLauncher : Tower
{
    public int attackCount = 5;

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data , int slotIndex)
    {
        base.Init(tower, manager, data , slotIndex);
        noise = 90f;
    }
    public override bool Attack()
    {
        Target = manager.FindTargetInRange(tower.transform.position, FullAttackRange)?.transform;
        base.Attack();
        return true;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Missile missile = Managers.ObjectPoolManager.SpawnObject<Missile>(PoolsId.Missile);
        //GameObject.Instantiate(attackprefab).GetComponent<Missile>();
        return missile;
    }
}
