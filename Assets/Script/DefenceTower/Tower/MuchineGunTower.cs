using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MuchineGunTower : Tower
{

    public override void Init(GameObject tower, TowerManager manager, TowerData.Data data)
    {
        base.Init(tower, manager, data);
        LoadProjectile().Forget();
    }

    private async UniTaskVoid LoadProjectile()
    {
        attackprefab = await Addressables.LoadAssetAsync<GameObject>("Bullet").ToUniTask();
        init = true;
    }

    public override bool Attack()
    {
        if (!init) return false;

        if(base.Attack())
        {
            ProjectTile projectile = GameObject.Instantiate(attackprefab).GetComponent<ProjectTile>();
            projectile.transform.position = tower.transform.position;

            projectile.Init(manager.FindTarget());
            return true;
        }

        return false;
    }
}
