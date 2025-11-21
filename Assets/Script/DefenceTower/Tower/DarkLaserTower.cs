using UnityEngine;

public class DarkLaserTower : Tower
{
    public override bool Attack()
    {
        CreateAttackPrefab();
        return true;
    }

    public override void Update(float deltaTime)
    {
        if (!UseAble)
            return;

        currentAttackInterval += deltaTime;
        if(currentAttackInterval >= BonusCoolTime + BonusDuration)
        {
            currentAttackInterval = 0;
            Attack();
        }
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var laser = Managers.ObjectPoolManager.SpawnObject<DarkLaser>(PoolsId.DarkLaser);
        laser.Init(this);
        return laser;
    }
}
