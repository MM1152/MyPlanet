using UnityEngine;

public class HellFireGunTower : MuchineGunTower
{
    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data)
    {
        base.Init(tower, manager, data);
        minNoise = -0.05f;
        maxNoise = 0.05f;
    }

    public override bool Attack()
    {
        target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    public override void LevelUp()
    {
        base.LevelUp();
        bonusAttackSpeed += 0.1f;
    }
}
