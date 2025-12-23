using UnityEngine;

public class BurstBlasterTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        if( attackAble && target != null )
        {
            for (int i = 0; i < BonusPelletCount; i++)
            {
                base.Attack();
                attackAble = true;
                //var attackPrefab = CreateAttackPrefab();
                //attackPrefab.Init(this);
                //attackPrefab.SetTarget(target, FullNoise * 0.5f);
                //attackPrefab.transform.position = this.tower.transform.position;
            }
            attackAble = false;
            currentAttackInterval = 0f;
            return true;
        }
        return false;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet bullet = Managers.ObjectPoolManager.SpawnObject<BurstBlasterBullet>(PoolsId.BurstBlasterBullet).GetComponent<Bullet>();
        Managers.SoundManager.PlaySFX(AudiosId.Flash_3);
        return bullet;
    }
}
