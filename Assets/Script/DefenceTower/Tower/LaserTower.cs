using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LaserTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Laser laser = GameObject.Instantiate(attackprefab).GetComponent<Laser>();

        return laser;
    }

    public override bool Attack()
    {
        target = manager.FindTarget()?.transform;
        return base.Attack();
    }
}
