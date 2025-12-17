using UnityEngine;

public class RepulsorTower : UtilTower
{
    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }

    public override bool Attack(bool useTarget = true)
    {
        CreateAttackPrefab();
        return true;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var repulsor = Managers.ObjectPoolManager.SpawnObject<Repulsor>(PoolsId.Repulsor);
        repulsor.Init(this);
        var dir = (tower.gameObject.transform.position - planet.transform.position).normalized;
        repulsor.transform.position = tower.transform.position/* + (dir * BonusAttackRange / 2f)*/;
        repulsor.SetDir(dir);
        return repulsor;
    }
}
