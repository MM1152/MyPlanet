using UnityEngine;

public class SurgeArresterTower : Tower
{
    private float timer = 0f;
    public override bool Attack(bool useTarget = true)
    {
        return base.Attack(false);
    }

    public override void Update(float deltaTime)
    {
        if (!UseAble) return;

        timer += deltaTime;
        if(timer >= BonusCoolTime + BonusDuration)
        {
            Attack();
            timer = 0;
            attackAble = true;
        }
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        return Managers.ObjectPoolManager.SpawnObject<Surge>(PoolsId.Surge);
    }
}
