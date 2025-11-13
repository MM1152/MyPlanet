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

    [SerializeField]
    private List<Vector3> spawnerList = new List<Vector3>();

    private Rect screenRect;

    // 스포너 갯수
    private const int topCount = 3;
    private const int rightCount = 4;
    private const int bottomCount = 3;
    private const int leftCount = 4;

    private const float addDis = 0.5f;
    

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
        //스포너 셋팅 
        SpawnaerSetting();
    }
    // 화면 크기 설정                                                         
    private void ScreenSizSet()
    {
        Camera mainCamera = Camera.main;

        float zDistance = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

        screenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    private void SpawnaerSetting()
    {
        ScreenSizSet();
        spawnerList.Clear();
        var xTopRatio = screenRect.width / (topCount+1);
        var xRightRatio = screenRect.height  / (rightCount+1);
        var xBottomRatio = screenRect.width / (bottomCount+1);
        var xLeftRatio = screenRect.height  / (leftCount+1);

        //위 시계방향으로 추가  25
        for (int i = 1; i <= topCount; i++)
        {
            spawnerList.Add(new Vector3(screenRect.xMin +xTopRatio*i, screenRect.yMax + addDis, 0f));
        }
        //오른쪽 Y -- 20
        for (int i = 1; i <= rightCount; i++)
        {
            spawnerList.Add(new Vector3(screenRect.xMax + addDis, screenRect.yMax-(xRightRatio * i), 0f));
        }
        //아래 25 
        for (int i = 1; i <= bottomCount; i++)
        {
            spawnerList.Add(new Vector3(screenRect.xMax - (xBottomRatio * i), screenRect.yMin - addDis, 0f));
        }
        //왼쪽 20 
        for (int i = 1; i <= leftCount; i++)
        {
            spawnerList.Add(new Vector3(screenRect.xMin - addDis,screenRect.yMin + (xLeftRatio * i), 0f));
        }        
    }

    // private void Update()
    // {
    //     if(Keyboard.current.spaceKey.wasPressedThisFrame)
    //     {
    //         SpawnEnemy(1);
    //     }
    // }

    public Enemy SpawnEnemy(int id, int index)
    {
        if (!init) return null;

        var data = DataTableManager.EnemyTable.GetData(id);

        if (data != null)
        {
            var spawnEnemy = poolManager.SpawnObject<Enemy>(id, enemyPrefab.gameObject);
            spawnEnemy.Initallized(data);
            //FIX : ���� ��ġ �ӽ� ����
            // spawnEnemy.transform.position = new Vector3(Random.Range(-5,5) , 5f , 0f);
            spawnEnemy.transform.position = spawnerList[index];
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
