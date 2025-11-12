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

    //FIX : 상속받은 자식에서 정의해주기
    public override bool Attack()
    {
        target = manager.FindTarget()?.transform;
        return base.Attack();
    }
}
