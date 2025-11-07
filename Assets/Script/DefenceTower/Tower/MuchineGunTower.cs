using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MuchineGunTower : Tower
{
    private GameObject projectilePrefab;
    private bool init = false;

    public override void Init(TowerManager manager, TowerData.Data data)
    {
        LoadProjectile().Forget();
        base.Init(manager, data);
    }

    private async UniTaskVoid LoadProjectile()
    {
        projectilePrefab = await Addressables.LoadAssetAsync<GameObject>("Bullet").ToUniTask();
        init = true;
    }

    public override bool Attack()
    {
        if (!init) return false;

        if(base.Attack())
        {
            ProjectTile projectile = GameObject.Instantiate(projectilePrefab).GetComponent<ProjectTile>();
            projectile.Init(manager.FindTarget());
            return true;
        }

        return false;
    }

    public override void Release()
    {
        if(projectilePrefab != null)
        {
            Addressables.Release(projectilePrefab);
        }
    }
}
