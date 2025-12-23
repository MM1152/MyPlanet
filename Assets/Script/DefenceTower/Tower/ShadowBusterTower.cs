using UnityEngine;

public class ShadowBusterTower : Tower
{
    public override bool Attack(bool useTarget = true)
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack(useTarget);
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
    }


    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var bullet = Managers.ObjectPoolManager.SpawnObject<ShadowBursterBullet>(PoolsId.ShadowBursterBullet);
        bullet.SetParticle(PoolsId.Hit23cube);
        Managers.SoundManager.PlaySFX(AudiosId.Flash_3);
        if(target != null)
        {
            var dir = (target.position - TowerGameObject.transform.position).normalized;

            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash23cube);
            flash.transform.position = TowerGameObject.transform.position + dir * TowerGameObject.transform.localScale.x;
        }
        return bullet;
    }
}