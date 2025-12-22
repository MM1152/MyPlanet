using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;

public class EnemyAssetManager : MonoBehaviour
{
    // 각각의 에셋이 담긴 프리팹은 
    [SerializeField] private Material redMaterial;//교체할 머티리얼들
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material defaultMaterial;
    private ObjectPoolManager poolManager;//오브젝트풀 매니저
    private PoolsId poolId;
    private List<EnemyAsset> spawnAssets = new List<EnemyAsset>(); //스폰된 에셋들

    private void Awake()
    {
        poolManager = Managers.ObjectPoolManager;
    }

    public void SpawnAsset(Enemy enemy)
    {
        if (enemy == null) return;

        if (enemy.enemyType == EnemyType.Boss)
        {
            poolId = BossAssetMap.GetBossAsset(enemy.enemyData.ID); //보스일경우 보스 에셋가져오기
        }
        else if (enemy.enemyType == EnemyType.EliteMonster)
        {
            poolId = EliteMonsterAssetMap.GetEliteMonsterAsset(enemy.ElementType); //엘리트 몬스터일경우 엘리트 몬스터 에셋가져오기
        }
        else
        {
            var tier = EnemyTier.GetTier(enemy.enemyData.ID); //티어가져오고 
            poolId = MonsterTierAssetMap.GetMonsterTierAsset(enemy.enemyType, tier); //풀 아이디 가져오기
        }
        EnemyAsset asset = poolManager.SpawnObject<EnemyAsset>(poolId); //에셋 풀링
        var material = SetMaterial(enemy.ElementType);
        asset.Init(enemy, material); //에셋 초기화
        enemy.OnDie += ClearAsset; //죽을때 에셋 제거
        enemy.enemyAssetPoolId = poolId; //풀 아이디 저장
        spawnAssets.Add(asset); //리스트에 추가
    }

    public Material SetMaterial(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.Fire => redMaterial,
            ElementType.Ice => blueMaterial,
            ElementType.Light => yellowMaterial,
            _ => defaultMaterial,
        };
    }

    public void ClearAsset(Enemy enemy)
    {
        var asset = enemy.enemyAsset;
        if (asset == null) return;

        asset.transform.SetParent(null);
        poolManager.Despawn(enemy.enemyAssetPoolId, asset.gameObject);
        spawnAssets.Remove(asset);
        enemy.enemyAsset = null;
        enemy.OnDie -= ClearAsset;
    }
}
