using UnityEngine;

public class HellFireGunTower : MuchineGunTower
{
    private float increaseAttackInterval = 1f;
    public override void Init(GameObject tower, TowerManager manager, TowerData.Data data)
    {
        base.Init(tower, manager, data);
        minNoise = -0.05f;
        maxNoise = 0.05f;
    }

    public override void LevelUp()
    {
        base.LevelUp();

        increaseAttackInterval += 0.1f;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime * increaseAttackInterval);
    }
}
