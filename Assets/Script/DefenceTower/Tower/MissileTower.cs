using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MissileTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Missile missile = GameObject.Instantiate(attackprefab).GetComponent<Missile>();
        missile.Init(towerData , typeEffectiveness);

        return missile;
    }
}
