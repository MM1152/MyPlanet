using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public class SpawnPoint
    {
        public Vector2 position;
        public int enemyId;
        public int spawnCount;
        public int maxSpawnCount;
        public float spawnStartDelayTime;
        public float spawnDelayTime;
        public float timer = 0f;
        public int currentSpawnEnemyCount = 0;
        public bool isStart = false;
    }
    private Dictionary<int, List<SpawnPoint>> waves = new Dictionary<int, List<SpawnPoint>>();
    private List<SpawnPoint> currentWave = new List<SpawnPoint>();
    private List<Vector2> spawnPoints = new List<Vector2>();
    private Rect screenBounds;
    private float spawnOffset = 1.0f;
    private int topPointCount = 3;
    private int leftPointCount = 4;
    private int bottomPointCount = 3;
    private int rightPointCount = 4;
    private int currentWaveIndex;
    public int CurrentWaveIndex => currentWaveIndex;
    private float waveDuration = 90f;
    public float WaveDuration => waveDuration;
    private float waveElapsedTime = 0f;
    public float WaveElapsedTime => waveElapsedTime;
    public int totalEnemyCount = 0;
    public bool isFinalWaveEnded = false;
    private EnemySpawnManager EnemySpawnManager;

    private bool isInitialized = false;

#if DEBUG_MODE
    public bool UIUpdateTest = false;
#endif

    private void Awake()
    {
        EnemySpawnManager = GameObject.FindWithTag(TagIds.EnemySpawnManagerTag).GetComponent<EnemySpawnManager>();
    }

    private void Start()
    {
        InitPoint();
        DataInit();
        currentWaveIndex = 0;
        isInitialized = true;
        currentWave.Clear();
        currentWave = waves[currentWaveIndex];
    }

    private void DataInit()
    {
        var waveData = DataTableManager.WaveTable.GetAllData();
        foreach (var wave in waveData)
        {
            int id = wave.Key;
            WaveData.Data data = wave.Value;
            int pointIndex = id % 100;
            int waveNumber = (id / 100) % 1000;

            if (pointIndex < 0 || pointIndex >= spawnPoints.Count)
            {
#if DEBUG_MODE                
                Debug.LogError($"Invalid point index {pointIndex} for Wave ID {id}");
#endif
            }

            var spawnPoint = new SpawnPoint()
            {
                position = spawnPoints[pointIndex],
                enemyId = data.Monster_ID,
                spawnCount = data.Spon,
                maxSpawnCount = data.Max_Spawn,
                spawnDelayTime = data.Spon_Time,
                spawnStartDelayTime = data.Spon_Cycle,
            };

            if (!waves.ContainsKey(waveNumber))
            {
                waves[waveNumber] = new List<SpawnPoint>();
            }

            waves[waveNumber].Add(spawnPoint);
        }
    }

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
            var y = screenBounds.yMin + rightInterval * (i + 1);
            spawnPoints.Add(new Vector2(x, y));
        }
        for (int i = 0; i < bottomPointCount; i++)
        {
            var x = screenBounds.xMin + bottomInterval * (i + 1);
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

    private void Update()
    {
        if (!isInitialized) return;

#if DEBUG_MODE
        UIUpdateTest = true;
#endif

        if (!waves.ContainsKey(currentWaveIndex))
        {
#if DEBUG_MODE
            Debug.Log($"Wave {currentWaveIndex} 데이터 없음");
#endif
            return;
        }

        waveElapsedTime += Time.deltaTime;

        if (waveElapsedTime >= waveDuration)
        {
            if (isFinalWaveEnded && totalEnemyCount <= 0)
            {
                EndGame();
            }
            else
            {
                NextWave();
            }
        }
        StartSpawnWave(Time.deltaTime);
    }

    private void StartSpawnWave(float deltaTime)
    {
        foreach (var spawnPoint in currentWave)
        {
            spawnPoint.timer += deltaTime;

            if (spawnPoint.currentSpawnEnemyCount >= spawnPoint.maxSpawnCount ||
            spawnPoint.spawnCount <= 0)
            {
                continue;
            }

            if (spawnPoint.timer >= spawnPoint.spawnStartDelayTime && !spawnPoint.isStart)
            {
                spawnPoint.isStart = true;
                spawnPoint.timer = 0f;
            }

            if (spawnPoint.timer >= spawnPoint.spawnDelayTime && spawnPoint.isStart)
            {
                var enemy = EnemySpawnManager.SpawnEnemy(spawnPoint.enemyId);
                if (enemy != null)
                {
                    enemy.transform.position = spawnPoint.position;
                    spawnPoint.spawnCount--;
                    totalEnemyCount++;
                    spawnPoint.currentSpawnEnemyCount++;
                    spawnPoint.timer = 0f;
                }
                else
                {
#if DEBUG_MODE
                    Debug.LogError($"Enemy ID {spawnPoint.enemyId} 소환 실패");
                    return;
#endif
                }
            }
        }
    }

    private void NextWave()
    {
        waveElapsedTime = 0f;
        int nextWaveIndex = currentWaveIndex + 1;

        if (!waves.ContainsKey(nextWaveIndex))
        {
            isFinalWaveEnded = true;
#if DEBUG_MODE
            Debug.Log("지금 마지막웨이브 다음으로 넘어갈수없다.");
#endif
            return;
        }

        var nextWave = waves[nextWaveIndex];

        foreach (var currentPoint in currentWave)
        {
            if (!currentPoint.isStart || (currentPoint.currentSpawnEnemyCount <= 0 && currentPoint.spawnCount <= 0))
                continue;


            var naxtPoint = nextWave.Find(p => p.position == currentPoint.position);

            if (naxtPoint != null)
            {
                naxtPoint.currentSpawnEnemyCount += currentPoint.currentSpawnEnemyCount;
                naxtPoint.spawnCount += currentPoint.spawnCount;
            }
            else
            {
                var previousWaveData = new SpawnPoint
                {
                    position = currentPoint.position,
                    enemyId = currentPoint.enemyId,
                    spawnCount = currentPoint.spawnCount,
                    maxSpawnCount = currentPoint.maxSpawnCount,
                    spawnDelayTime = currentPoint.spawnDelayTime,
                    spawnStartDelayTime = 0f, // 즉시 시작
                    currentSpawnEnemyCount = currentPoint.currentSpawnEnemyCount,
                    isStart = true
                };
                nextWave.Add(previousWaveData);
            }
        }

        currentWave.Clear();

        currentWave = nextWave;

        currentWaveIndex = nextWaveIndex;

        waveElapsedTime = 0f;
    }

    private void EndGame()
    {
        // 여기에 게임 종료 로직 추가
    }
}
