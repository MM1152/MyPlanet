using UnityEngine;

public class MuchineGunTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet projectile = Managers.ObjectPoolManager.SpawnObject<Bullet>(PoolsId.Bullet);
        //Bullet projectile = GameObject.Instantiate(attackprefab).GetComponent<Bullet>();
        return projectile;
    }
}

