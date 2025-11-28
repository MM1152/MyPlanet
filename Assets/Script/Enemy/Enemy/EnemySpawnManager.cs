using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class EnemySpawnManager : MonoBehaviour
{
    private WindowManager windowManager;
    private ObjectPoolManager poolManager;
    private List<Enemy> spawnEnemys = new List<Enemy>();
    private bool init = false;
#if DEBUG_MODE
    public Button testButton;
#endif

    public bool isDebugMode = false;

    private void InitalizedAsync()
    {
        //FIX: ������ ���̺� �ʱ�ȭ �ȱ�ٸ�����
        //await DataTableManager.WaitForInitalizeAsync();
        //var enemy = await Addressables.LoadAssetAsync<GameObject>("Enemy").ToUniTask();
        //enemyPrefab = enemy.GetComponent<Enemy>();

        //init = true;
#if DEBUG_MODE
        testButton?.gameObject.SetActive(true);
        testButton?.onClick.AddListener(() => SpawnEnemy(1));
#endif
    }
    private void Awake()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag)?.GetComponent<WindowManager>();
        poolManager = Managers.ObjectPoolManager;
        InitalizedAsync();
    }
 
    public List<Enemy> SpawnEnemy(int id, int count = 1)
    {
        //if (!init) return null;

        var spawnedEnemies = new List<Enemy>();
        var data = DataTableManager.EnemyTable.GetData(id);

        if (data != null)
        {
            for (int i = 0; i < count; i++)
            {
                var spawnEnemy = poolManager.SpawnObject<Enemy>(PoolsId.Enemy);
                if (EnemyTypes.IsEliteMonster(id))
                {
                    spawnEnemy.OnDie += (enemy) => windowManager?.Open(WindowIds.OptionUpgradeWindow);
                }
                spawnEnemy.Initallized(data);      
                if(isDebugMode)
                {
                    spawnEnemy.DebugToolsInit();
                }
                spawnEnemy.OnDie += CheckDieEnemy;
                spawnEnemys.Add(spawnEnemy);
                spawnedEnemies.Add(spawnEnemy); 
            }
            return spawnedEnemies;
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
