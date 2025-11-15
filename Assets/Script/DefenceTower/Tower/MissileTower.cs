using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MissileTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Missile missile = Managers.ObjectPoolManager.SpawnObject<Missile>(PoolsId.Missile);
        //GameObject.Instantiate(attackprefab).GetComponent<Missile>();
        return missile;
    }
}
