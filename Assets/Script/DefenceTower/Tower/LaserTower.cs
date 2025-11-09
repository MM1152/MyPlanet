using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LaserTower : Tower
{
    public override void Init(GameObject tower, TowerManager manager, TowerData.Data data)
    {
        base.Init(tower, manager, data);
        LoadProjectTileAsync().Forget();
    }

    private async UniTaskVoid LoadProjectTileAsync()
    {
        attackprefab = await Addressables.LoadAssetAsync<GameObject>("Laser").ToUniTask();
        init = true;
    }
    
    public override bool Attack()
    {
        if(base.Attack())
        {
            Laser laser = GameObject.Instantiate(attackprefab).GetComponent<Laser>();
            laser.transform.position = tower.transform.position;
            laser.Init(target);
            
            return true;
        }
        return false;
    }
}
