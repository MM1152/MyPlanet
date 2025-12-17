using UnityEngine;

public class ShadowSurgeTower : Tower
{
    private float timer;
    public override bool Attack(bool useTarget = true)
    {
        timer += Time.deltaTime;
        if (timer < BonusCoolTime) return false;

        timer = 0;
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