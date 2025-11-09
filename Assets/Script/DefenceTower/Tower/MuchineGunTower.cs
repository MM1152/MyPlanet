using UnityEngine;

public class MuchineGunTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Bullet projectile = GameObject.Instantiate(attackprefab).GetComponent<Bullet>();

        return projectile;
    }
}

