using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class EnemySpawnManager : MonoBehaviour
{
    private ObjectPoolManager poolManager;
    private Enemy enemyPrefab;
    private List<Enemy> spawnEnemys = new List<Enemy>();
    private bool init = false;

    private async UniTaskVoid InitalizedAsync()
    {      
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

    public Enemy SpawnEnemy(int id)
    {
        if (!init) return null;

        var data = DataTableManager.EnemyTable.GetData(id);

        if (data != null)
        {
            var spawnEnemy = poolManager.SpawnObject<Enemy>(1, enemyPrefab.gameObject);
            spawnEnemy.Initallized(data);           
            spawnEnemy.OnDie += CheckDieEnemy;
            spawnEnemys.Add(spawnEnemy);

            return spawnEnemy;
        }
        return null;
    }

    private void CheckDieEnemy(Enemy enemy)
    {
        spawnEnemys.Remove(enemy);
    }

    // target���κ��� �Ÿ��� ���� ª�� Ÿ�� �����ֱ�
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
        if (list != null && list.Count != 0)
        {
            return list[0];
        }
        return null;
    }
}
