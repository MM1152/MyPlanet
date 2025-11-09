using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MissileTower : Tower
{
    public override void Init(GameObject tower, TowerManager manager, TowerData.Data data)
    {
        base.Init(tower, manager, data);
        LoadProjectTile().Forget();
    }

    public async UniTask LoadProjectTile()
    {
        attackprefab = await Addressables.LoadAssetAsync<GameObject>("Missile").ToUniTask();
        init = true;
    }

    public override bool Attack()
    {
        if(base.Attack())
        {
            Missile missile = GameObject.Instantiate(attackprefab).GetComponent<Missile>();
            missile.transform.position = tower.transform.position;
            missile.Init(target);
            
            return true;
        }

        return false;
    }
}
