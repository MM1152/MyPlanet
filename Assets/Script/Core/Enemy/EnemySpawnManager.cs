using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class EnemySpawnManager : MonoBehaviour
{
    private ObjectPoolManager poolManager;
    private Enemy enemyPrefab;
    private List<Enemy> spawnEnemys = new List<Enemy>();
    private bool init = false;

    private async UniTaskVoid InitalizedAsync()
    {
        //FIX: 데이터 테이블 초기화 안기다릴꺼임
        await DataTableManager.WaitForInitalizeAsync();
        var enemy = await Addressables.LoadAssetAsync<GameObject>("Enemy").ToUniTask();
        enemyPrefab = enemy.GetComponent<Enemy>();

        init = true;
    }

    private void Awake()
    {
        poolManager = ObjectPoolManager.Instance;
        InitalizedAsync().Forget();
    }

    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnEnemy(1);
        }
    }

    public Enemy SpawnEnemy(int id)
    {
        if (!init) return null;

        var data = DataTableManager.EnemyTable.GetData(id);

        if(data != null)
        {
            var spawnEnemy = poolManager.SpawnObject<Enemy>(id , enemyPrefab.gameObject);
            spawnEnemy.Initallized(data);
            //FIX : 스폰 위치 임시 지정
            spawnEnemy.transform.position = new Vector3(Random.Range(-5,5) , 5f , 0f);
            spawnEnemy.OnDie += CheckDieEnemy;
            spawnEnemys.Add(spawnEnemy);

            return spawnEnemy;
        }
        return null;
    }

    private void CheckDieEnemy(Enemy enemy)
    {
        enemy.OnDie -= CheckDieEnemy;
        spawnEnemys.Remove(enemy);
    }

    // target으로부터 거리가 가장 짧은 타겟 돌려주기
    public List<Enemy> GetEnemyDatas(Vector3 position)
    {
        if (spawnEnemys.Count == 0) return null;

        List<Enemy> copyList = new List<Enemy>(spawnEnemys);

        copyList.Sort((a, b) => 
        {
            float distA = Vector3.Distance(a.transform.position, position);
            float distB = Vector3.Distance(b.transform.position, position);
            return distA.CompareTo(distB);
        });

        return copyList;
    }

    public Enemy GetEnemyData(Vector3 position)
    {
        var list = GetEnemyDatas(position);
        if(list != null && list.Count != 0)
        {
            return list[0];
        }
        return null;
    }
}
