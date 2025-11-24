using UnityEngine;

public class ShadowSurgeTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        for(int i = 0; i < BonusProjectileCount; i++)
        {
            base.Attack(false);
            attackAble = true;
        }
        attackAble = false;
        return true;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var bullet = Managers.ObjectPoolManager.SpawnObject<ShadowSurge>(PoolsId.ShadowSurge);
        return bullet;
    }
}