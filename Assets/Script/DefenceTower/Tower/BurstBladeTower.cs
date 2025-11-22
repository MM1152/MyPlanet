using UnityEngine;

public class BurstBlasterTower : Tower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        if( attackAble && target != null )
        {
            for (int i = 0; i < BonusPelletCount; i++)
            {
                var attackPrefab = CreateAttackPrefab();
                attackPrefab.Init(this);
                attackPrefab.SetTarget(target, FullNoise * 0.5f);
                attackPrefab.transform.position = this.tower.transform.position;
            }
            attackAble = false;
            currentAttackInterval = 0f;
            return true;
        }
        return false;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var bullet = Managers.ObjectPoolManager.SpawnObject<Bullet>(PoolsId.Bullet).GetComponent<Bullet>();
        return bullet;
    }
}
