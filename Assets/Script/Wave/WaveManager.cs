using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
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
    private float waveDuration = 10f;
    public float WaveDuration => waveDuration;
    private float waveElapsedTime = 0f;
    public float WaveElapsedTime => waveElapsedTime;
    public bool isFinalWaveEnded => currentWaveIndex >= waves.Count;

    public int waveClearCount = 0;
    public int totalEnemyCount = 0;
    private EnemySpawnManager enemySpawnManager;
    public EnemySpawnManager EnemySpawnManager => enemySpawnManager;


    [Header("References")]
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private TutorialManager tutorialManager;
    public int NextTutorialWaveIndex { get; set; } = 5;

    private float playTimeTimer;
#if DEBUG_MODE
    public bool UIUpdateTest = false;
#endif
    private float terraformingPercentThreshold = 0.94f;
    public int terraformingValueCount => (int)(waves.Count * terraformingPercentThreshold) < 1 ? 1 : (int)(waves.Count * terraformingPercentThreshold);

    [SerializeField]
    private GameObject warringWindow;
    [SerializeField]
    private TextMeshProUGUI warringText;

    [SerializeField]
    private WaveWindow waveWindow;

    private int stageId = 1;

    private void Awake()
    {
        stageId = FirebaseManager.Instance.PresetData.GetGameData().stageId;
        enemySpawnManager = GameObject.FindWithTag(TagIds.EnemySpawnManagerTag).GetComponent<EnemySpawnManager>();
    }

    private void Start()
    {
        InitPoint();
        DataInit();
        ResetWave();
        TerraformingData.terraformingUnlockPoints.Clear();
#if DEBUG_MODE
        UIUpdateTest = true;
#endif        
    }

    private void ResetWave()
    {
        currentWave.Clear();
        currentWaveIndex = 1;
        totalEnemyCount = 0;
        waveElapsedTime = 0f;
        currentWave = waves[currentWaveIndex];
        waveWindow.SetWaveText(currentWaveIndex);
        foreach (var spawnPoint in currentWave)
        {
            spawnPoint.timer = 0f;
            spawnPoint.isStart = false;
            spawnPoint.currentSpawnEnemyCount = 0;
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
            for (int i = 1; i <= terraformingValueCount; i++)
            {
                if (!waves.ContainsKey(i)) continue;

                var wave = waves[i];
                foreach (var spawnPoint in wave)
                {
                    totalTerraformingValue += spawnPoint.maxSpawnCount;
                }
            }
            waveTerraformingValue = 0;
            sliderValue.UpdateSlider(waveTerraformingValue, totalTerraformingValue, waveTerraformingValue * 100 / totalTerraformingValue);
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
        if (!Variable.IsSpawnActive) return;
        playTimeTimer += Time.deltaTime;
        if (!isFinalWaveEnded)
        {
            waveElapsedTime += Time.deltaTime;
            waveWindow.SetWaveTimerText(waveDuration - waveElapsedTime);
        }
        else
        {
            waveWindow.SetWaveTimerText(0f);
        }


        if ((!isFinalWaveEnded && waveElapsedTime >= waveDuration) || waveClearCount <= 0)
        {
            if (isFinalWaveEnded)
            {
                EndGame(true);
            }
            else
            {
                NextWave();
            }
        }
        StartSpawnWave(Time.deltaTime);
    }

    public void StartSpawnWave(float deltaTime)
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
        // terraformingValueCount 범위 내의 웨이브에서만 terraforming 값 증가
        // if (currentWaveIndex > terraformingValueCount)
        //     return;

        if (waveTerraformingValue >= totalTerraformingValue)
            return;

        waveTerraformingValue++;
        float percent = Mathf.Min((float)waveTerraformingValue / totalTerraformingValue, 1f) * 100f;
        sliderValue.UpdateSlider(waveTerraformingValue, totalTerraformingValue, (int)(percent));
        Debug.Log($"Terraforming Value Updated: {waveTerraformingValue}/{totalTerraformingValue} ({percent}%)");

        for (int i = 0; i < TerraformingData.terrformingOpenValues.Length; i++)
        {
            if (percent >= TerraformingData.terrformingOpenValues[i])
            {
                if (!TerraformingData.terraformingUnlockPoints.Contains(TerraformingData.terrformingOpenValues[i]))
                {
                    TerraformingData.terraformingUnlockPoints.Add(TerraformingData.terrformingOpenValues[i]);
                    terraforming.SetPoint(i + 1);
                    return;
                }
            }
        }
    }
    //Tutorial
    public void StartWave()
    {
        StartSpawnWave(Time.deltaTime);
    }
    public void NextWave()
    {
        int nextWaveIndex = currentWaveIndex + 1;
        if (Variable.IsTutorialActive && tutorialManager != null && NextTutorialWaveIndex == currentWaveIndex)
        {
            tutorialManager.SetSectorTutorial(NextTutorialWaveIndex);
        }
        if (!waves.ContainsKey(nextWaveIndex))
        {
            return;
        }

        currentWave.Clear();
        currentWave = waves[nextWaveIndex];
        currentWaveIndex = nextWaveIndex;
        waveWindow.SetWaveText(currentWaveIndex);
        foreach (var currentPoint in currentWave)
        {
            waveClearCount += currentPoint.maxSpawnCount;
            currentPoint.timer = 0f;
            currentPoint.isStart = false;
        }
        waveElapsedTime = 0f;

        var monsterId = currentWave[0].enemyId;
        if (EnemyTypes.IsEliteMonster(monsterId))
        {
            warringWindow.SetActive(true);
            warringText.text = $"<i>엘리트 보스 몬스터 출현!<i>";
            ShowWarringWindow().Forget();
            Time.timeScale = 0f;
        }
        else if (EnemyTypes.IsBossMonster(monsterId))
        {
            warringWindow.SetActive(true);
            warringText.text = $"<i>보스 몬스터 출현!<i>";
            ShowWarringWindow().Forget();
            Time.timeScale = 0f;
        }

    }

    public void EndGame(bool isClear)
    {
        // 여기에 게임 종료 로직 추가
        if (windowManager != null)
        {
            var window = windowManager.Open(WindowIds.VictoryWindow);
            if (window is VictoryWindow victoryWindow)
            {
                victoryWindow.SetVictoryUI(isClear);
                victoryWindow.UpdateText(playTimeTimer, isClear);
            }
        }
        Time.timeScale = 0f;
        Debug.Log("게임 종료");
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
