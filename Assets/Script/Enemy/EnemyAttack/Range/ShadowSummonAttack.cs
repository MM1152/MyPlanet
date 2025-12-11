using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

public class ShadowSummonAttack : IShotStrategy
{
    private class SummonInfo
    {
        public bool active;
        public int enemyId;
        public int spawnCount;
        public float spawnInterval;

    }
    private Dictionary<int, SummonInfo> summonInfos = new Dictionary<int, SummonInfo>()
    {
        {0, new SummonInfo{ active = false, enemyId = 3005, spawnCount = 5, spawnInterval = 2f } }, // 티어 3
        {1, new SummonInfo{ active = false, enemyId = 3010, spawnCount = 7, spawnInterval = 2f } }, // 티어 2
        {2, new SummonInfo{ active = false, enemyId = 3020, spawnCount = 8, spawnInterval = 1f } }, // 티어 1
    };
    Enemy enemy;
    private int[] spawnPositions = { 0, 3 }; //테이블 연결
    private float[] phaseThresholds = { 1F, 0.5f, 0.25f }; //테이블 연결 

    private bool isInitialized = false;

#if DEBUG_MODE
    private List<Vector2> spawnPoints = new List<Vector2>();
    private Rect screenBounds;
    private int topPointCount = 3;
    private int leftPointCount = 4;
    private int bottomPointCount = 3;
    private int rightPointCount = 4;
    private float spawnOffset = 1.0f;
    List<Enemy> shadows = new List<Enemy>();
#endif

    public void Shot(Enemy enemy, GameObject target)
    {
        if (enemy == null) return;

        if (!isInitialized)
        {
            this.enemy = enemy;
            isInitialized = true;
            #if DEBUG_MODE
            if(enemy.WaveManager == null)
            {
                InitPoint();
            }
            #endif
        }

        float healthPercent = (float)enemy.currentHP / enemy.enemyData.HP;
        for (int i = 0; i < phaseThresholds.Length; i++)
        {
            if (healthPercent <= phaseThresholds[i] && !summonInfos[i].active)
            {
                summonInfos[i].active = true;
                SummonShadows(summonInfos[i]).Forget();
            }
        }
    }

    private async UniTask SummonShadows(SummonInfo info)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(info.spawnInterval), ignoreTimeScale: false, cancellationToken: enemy.GetCancellationTokenOnDestroy());
#if DEBUG_MODE
        if (enemy.WaveManager == null)
        {
            var enemySpawnManager = GameObject.FindWithTag(TagIds.EnemySpawnManagerTag).GetComponent<DebugEnemySpawnManager>();
            shadows = enemySpawnManager.SpawnEnemy(info.enemyId, info.spawnCount);
        }
        else
        {
#endif
            shadows = enemy.WaveManager.EnemySpawnManager.SpawnEnemy(info.enemyId, info.spawnCount);
        }

        foreach (var shadow in shadows)
        {
            var index = Random.Range(0, spawnPositions.Length);
            Debug.Log($"소환 위치 인덱스: {index}");
#if DEBUG_MODE
            if (enemy.WaveManager == null)
            {
                shadow.transform.position = spawnPoints[spawnPositions[index]] + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                Debug.Log($"소환 위치: {spawnPositions[index]}");
            }
            else
            {
#endif
                shadow.transform.position = enemy.WaveManager.SpawnPoints[spawnPositions[index]] + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }
            Debug.Log($"소환 위치: {spawnPositions[index]}");
            shadow.move.Init(shadow);
        }
        info.active = false;
    }
#if DEBUG_MODE
    private void InitScreenBounds()
    {
        var camera = Camera.main;

        if (camera == null) return;

        var zDistance = Mathf.Abs(camera.transform.position.z);

        var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
        var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

        screenBounds = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }
    private void InitPoint()
    {
        InitScreenBounds();
        spawnPoints.Clear();

        var topInterval = (screenBounds.width) / (topPointCount + 1);
        var rightInterval = (screenBounds.height) / (rightPointCount + 1);
        var bottomInterval = (screenBounds.width) / (bottomPointCount + 1);
        var leftInterval = (screenBounds.height) / (leftPointCount + 1);

        for (int i = 0; i < topPointCount; i++)
        {
            var x = screenBounds.xMin + topInterval * (i + 1);
            var y = screenBounds.yMax + spawnOffset;
            spawnPoints.Add(new Vector2(x, y));
        }
        for (int i = 0; i < rightPointCount; i++)
        {
            var x = screenBounds.xMax + spawnOffset;
            var y = screenBounds.yMax - rightInterval * (i + 1);
            spawnPoints.Add(new Vector2(x, y));
        }
        for (int i = 0; i < bottomPointCount; i++)
        {
            var x = screenBounds.xMax - bottomInterval * (i + 1);
            var y = screenBounds.yMin - spawnOffset;
            spawnPoints.Add(new Vector2(x, y));
        }
        for (int i = 0; i < leftPointCount; i++)
        {
            var x = screenBounds.xMin - spawnOffset;
            var y = screenBounds.yMin + leftInterval * (i + 1);
            spawnPoints.Add(new Vector2(x, y));
        }
    }
#endif
}
