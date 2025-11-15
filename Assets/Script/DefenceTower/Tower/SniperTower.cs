using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SniperTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        SniperBullet sniperBullet = Managers.ObjectPoolManager.SpawnObject<SniperBullet>(PoolsId.SniperBullet);
        return sniperBullet;
    }
}
