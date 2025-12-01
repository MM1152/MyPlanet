using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
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
        public int currentSpawnEnemyCount;
        public bool isStart = false;
    }
    private Dictionary<int, List<SpawnPoint>> waves = new Dictionary<int, List<SpawnPoint>>();
    private List<SpawnPoint> currentWave = new List<SpawnPoint>();
    private List<Vector2> spawnPoints = new List<Vector2>();

    [SerializeField]
    private SliderValue sliderValue;

    [SerializeField]
    private Terraforming terraforming;
    //현재량 
    private int waveTerraformingValue = 0;
    //총량
    private int totalTerraformingValue = 0;

    private Rect screenBounds;
    public Rect ScreenBounds => screenBounds;

    private float spawnOffset = 1.0f;
    private int topPointCount = 3;
    private int leftPointCount = 4;
    private int bottomPointCount = 3;
    private int rightPointCount = 4;

    private int nextWaveIndex = 1;
    private int currentWaveIndex = 1;
    public int CurrentWaveIndex => currentWaveIndex;
    private float waveDuration = 20f;
    public float WaveDuration => waveDuration;
    private float waveElapsedTime = 0f;
    public float WaveElapsedTime => waveElapsedTime;
    public bool isFinalWaveEnded = false;

    public int waveClearCount = 0;
    public int totalEnemyCount = 0;
    private EnemySpawnManager enemySpawnManager;
    public EnemySpawnManager EnemySpawnManager => enemySpawnManager;

#if DEBUG_MODE
    public bool UIUpdateTest = false;
#endif

    public int terrformingValueCount = 56;

    public int[] monsterStage = new int[2] { 2, 3 };

    [SerializeField]
    private GameObject warringWindow;

    [SerializeField]
    private WaveWindow waveWindow;

    private int stageId = 1;
    public int SetStageId(int id)
    {
        stageId = id;
        return stageId;
    }

    private void Awake()
    {
        enemySpawnManager = GameObject.FindWithTag(TagIds.EnemySpawnManagerTag).GetComponent<EnemySpawnManager>();
    }

    private void Start()
    {
        InitPoint();
        DataInit();
        ResetWave();
#if DEBUG_MODE
        UIUpdateTest = true;
#endif
        waveTerraformingValue = 0;

        sliderValue.UpdateSlider(waveTerraformingValue, totalTerraformingValue, waveTerraformingValue * 100 / totalTerraformingValue);
    }

    private void ResetWave()
    {
        currentWave.Clear();
        currentWaveIndex = 1;
        totalEnemyCount = 0;
        waveElapsedTime = 0f;
        isFinalWaveEnded = false;
        currentWave = waves[currentWaveIndex];
        waveWindow.SetWaveText(currentWaveIndex);
        foreach (var spawnPoint in currentWave)
        {
            spawnPoint.timer = 0f;
            spawnPoint.isStart = false;
            waveClearCount += spawnPoint.maxSpawnCount;
        }
    }

    private void DataInit()
    {
        waves.Clear();
        var stageData = DataTableManager.WaveTable.GetStageData(stageId);

        if (stageData == null)
        {
#if DEBUG_MODE
            Debug.LogError($"StageData ID {stageId} 데이터를 찾을수없다.");
#endif
            return;
        }

        foreach (var waveGroup in stageData.waveGroups)
        {
            var waveNumber = waveGroup.waveIndex;
            foreach (var data in waveGroup.waveDatas)
            {
                var spawnPoint = new SpawnPoint()
                {
                    position = spawnPoints[data.SPON_POINT],
                    enemyId = data.MON_ID,
                    spawnCount = data.SPON_COUNT,
                    maxSpawnCount = data.MAX_SPON,
                    spawnDelayTime = data.INTERVAL,
                    spawnStartDelayTime = data.SPON_TIME,
                };

                if (!waves.ContainsKey(waveNumber))
                {
                    waves[waveNumber] = new List<SpawnPoint>();
                }
                waves[waveNumber].Add(spawnPoint);
            }
        }



        if (stageData != null)
        {
            foreach (var waveGroup in stageData.waveGroups)
            {
                for (int i = 0; i < terrformingValueCount; i++)
                {
                    if (waveGroup.waveDatas.Count <= i) break;
                    totalTerraformingValue += waveGroup.waveDatas[i].MAX_SPON;
                }
            }
        }
        else
        {
            totalTerraformingValue = 0;
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
        waveElapsedTime += Time.deltaTime;

        if (waveElapsedTime >= waveDuration || waveClearCount <= 0)
        {
            if (isFinalWaveEnded)
            {
                EndGame();
            }
            else
            {
                NextWave();
            }
        }
        StartSpawnWave(Time.deltaTime);
        waveWindow.SetWaveTimerText(waveDuration - waveElapsedTime);
    }

    private void StartSpawnWave(float deltaTime)
    {
        foreach (var spawnPoint in currentWave)
        {
            spawnPoint.timer += deltaTime;

            if (spawnPoint.currentSpawnEnemyCount >= spawnPoint.maxSpawnCount)
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
                var remainingToSpawn = spawnPoint.maxSpawnCount - spawnPoint.currentSpawnEnemyCount;
                var minCount = Mathf.Min(spawnPoint.spawnCount, remainingToSpawn);

                var enemys = enemySpawnManager.SpawnEnemy(spawnPoint.enemyId, minCount);
                spawnPoint.currentSpawnEnemyCount += minCount;
                totalEnemyCount += minCount;
                if (enemys != null)
                {
                    foreach (var enemy in enemys)
                    {
                        var offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                        enemy.transform.position = spawnPoint.position + offset;
                        enemy.move.Init(enemy);
                        enemy.OnTerraformingValueChanged += () =>
                        {
                            UpdateTerraformingValue();
                        };
                    }
                    spawnPoint.timer = 0f;
                }
                else
                {
#if DEBUG_MODE
                    return;
#endif
                }
            }
        }
    }

    public void UpdateTerraformingValue()
    {
        waveTerraformingValue++;
        var percent = waveTerraformingValue * 100 / totalTerraformingValue;
        sliderValue.UpdateSlider(waveTerraformingValue, totalTerraformingValue, percent);

        for (int i = 0; i < TerraformingData.terrformingOpenValues.Length; i++)
        {
            if (percent >= TerraformingData.terrformingOpenValues[i])
            {
                if (!TerraformingData.terraformingUnlockPoints.Contains(TerraformingData.terrformingOpenValues[i]))
                {
                    TerraformingData.terraformingUnlockPoints.Add(TerraformingData.terrformingOpenValues[i]);
                    terraforming.SetPoint(i + 1);
                    Time.timeScale = 0f;
                    return;
                }
            }
        }
    }

    private void NextWave()
    {
        nextWaveIndex = currentWaveIndex + 1;

        if (!waves.ContainsKey(nextWaveIndex))
        {
            if (waves.Count <= nextWaveIndex)
            {
                isFinalWaveEnded = true;
            }

#if DEBUG_MODE
            Debug.Log("다음 웨이브가 없습니다.");
#endif
            return;
        }

        currentWave.Clear();
        currentWave = waves[nextWaveIndex];
        currentWaveIndex = nextWaveIndex;
        waveWindow.SetWaveText(currentWaveIndex);
        foreach (var currentPoint in currentWave)
        {
            waveClearCount += currentPoint.maxSpawnCount;
        }
        waveElapsedTime = 0f;

        if (monsterStage.Contains(currentWaveIndex))
        {
            warringWindow.SetActive(true);
            ShowWarringWindow().Forget();
            Time.timeScale = 0f;
        }
    }

    private void EndGame()
    {
        // 여기에 게임 종료 로직 추가
    }

    private async UniTask ShowWarringWindow()
    {
        await UniTask.Delay(1000, true, cancellationToken: this.GetCancellationTokenOnDestroy());
        Time.timeScale = 1.0f;
        warringWindow.SetActive(false);
        if (waves.Count <= currentWaveIndex)
        {
            enemySpawnManager.ClearAllEnemy();
        }
    }
}
