using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public class SpawnPoint
    {
        // 스폰위치
        public Vector2 position;
        // 스폰될 적 타입
        public int enemyId;
        // 현 웨이브 현 포인트에서 스폰될 적 숫자
        public int spawnCount;
        // 각 포인트가 유지할 스폰 최대숫자 
        public int maxSpawnCount;
        // 스폰 시작까지의 딜레이타임
        public float spawnStartDelayTime;
        // 스폰 딜레이 타임
        public float spawnDelayTime;
        // 딜레이 비교용 타이머
        public float timer = 0f;
        // 현재 스폰된 적 숫자
        public int currentSpawnEnemyCount = 0;
        // 스타트될 준비
        public bool isStart = false;
    }
    // 스폰 위치 포인트 인덱스와 소환될 적 과 소환할 숫자 관리
    private Dictionary<int, List<SpawnPoint>> waves = new Dictionary<int, List<SpawnPoint>>();
    // 현재 진행중인 웨이브의 스폰포인트 리스트
    private List<SpawnPoint> currentWave = new List<SpawnPoint>();
    // ===================================    스폰위치 셋팅용
    // 스폰포인트 담을곳
    private List<Vector2> spawnPoints = new List<Vector2>();
    // 화면 경계 담을곳
    private Rect screenBounds;
    // 환경 경계 추가 값
    private float spawnOffset = 1.0f;
    // 각 상,하,좌,우 스폰 포인트 갯수
    private int topPointCount = 3;
    private int leftPointCount = 4;
    private int bottomPointCount = 3;
    private int rightPointCount = 4;
    // ===================================    웨이브 상태 관리용
    // 현재 웨이브
    private int currentWaveIndex;
    // 현재 웨이브 프로퍼티
    public int CurrentWaveIndex => currentWaveIndex;
    // 웨이브 지속 시간 (임시)
    private float waveDuration = 5f;
    // 웨이브 경과 시간
    private float waveElapsedTime = 0f;
    // 총 소환된 적 숫자
    public int totalEnemyCount = 0;
    // 마지막 웨이브 활성화
    public bool isFinalWaveEnded = false;
    // 적 소환 매니져
    private EnemySpawnManager EnemySpawnManager;

    private bool isInitialized = false;

    private void Awake()
    {
        // 스폰매니져 가져오고
        EnemySpawnManager = GameObject.FindWithTag(TagIds.EnemySpawnManagerTag).GetComponent<EnemySpawnManager>();
    }

    // 시작할때 초기화
    private void Start()
    {
        // 화면 경계 초기화
        InitPoint();
        //데이터 테이블에서 웨이브 데이터 불러와 세팅
        DataInit();
        //현재 웨이브 초기화
        currentWaveIndex = 0;
        isInitialized = true;
        currentWave.Clear();
        currentWave = waves[currentWaveIndex];
    }

    //데이터 테이블 불러와서 세팅 
    private void DataInit()
    {
        //테이블 가져옴 
        var waveData = DataTableManager.WaveTable.GetAllData();
        foreach (var wave in waveData)
        {
            // 테이블 분리들어갑니다.
            int id = wave.Key;
            WaveData.Data data = wave.Value;
            // 테이블 아이디 2자리 끊어서 포인트 인덱스 구하기 
            int pointIndex = id % 100;
            // 3자리 끊어서 웨이브 번호 구하기
            int waveNumber = (id / 100) % 1000;

            if (pointIndex < 0 || pointIndex >= spawnPoints.Count)
            {
#if DEBUG_MODE
                // 포인터 인덱스 벗어났을때 오류
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

            // 웨이브 번호에 해당하는 스폰포인트 리스트가 없으면 새로 생성
            if (!waves.ContainsKey(waveNumber))
            {
                waves[waveNumber] = new List<SpawnPoint>();
            }
            // 데이터 할당
            waves[waveNumber].Add(spawnPoint);
        }
    }
    // 화면 경계 초기화
    private void InitScreenBounds()
    {
        //메인 카메라 가져와
        var camera = Camera.main;
        // 카메라가 없으면 리턴
        if (camera == null) return;
        //카메라와 오브젝트 사이의 z거리 계산
        var zDistance = Mathf.Abs(camera.transform.position.z);
        //화면의 네 구석의 월드 좌표 계산
        var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
        var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));
        //화면 경계 사각형 설정
        screenBounds = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }
    private void InitPoint()
    {
        InitScreenBounds();
        spawnPoints.Clear();

        // 포인트별 간격 계산
        var topInterval = (screenBounds.width) / (topPointCount + 1);
        var rightInterval = (screenBounds.height) / (rightPointCount + 1);
        var bottomInterval = (screenBounds.width) / (bottomPointCount + 1);
        var leftInterval = (screenBounds.height) / (leftPointCount + 1);

        // 탑 포인트 생성
        for (int i = 0; i < topPointCount; i++)
        {
            var x = screenBounds.xMin + topInterval * (i + 1);
            var y = screenBounds.yMax + spawnOffset;
            spawnPoints.Add(new Vector2(x, y));
        }
        // 오른쪽 포인트 생성
        for (int i = 0; i < rightPointCount; i++)
        {
            var x = screenBounds.xMax + spawnOffset;
            var y = screenBounds.yMin + rightInterval * (i + 1);
            spawnPoints.Add(new Vector2(x, y));
        }
        // 바텀 포인트 생성
        for (int i = 0; i < bottomPointCount; i++)
        {
            var x = screenBounds.xMin + bottomInterval * (i + 1);
            var y = screenBounds.yMin - spawnOffset;
            spawnPoints.Add(new Vector2(x, y));
        }
        // 왼쪽 포인트 생성
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

        // 해당 웨이브가 없으면 리턴
        if (!waves.ContainsKey(currentWaveIndex))
        {
#if DEBUG_MODE
            Debug.Log($"Wave {currentWaveIndex} 데이터 없음");
#endif
            return; // 해당 웨이브가 없으면 종료
        }

        waveElapsedTime += Time.deltaTime;


        // 웨이브 지속 시간이 경과했는지 확인
        if (waveElapsedTime >= waveDuration)
        {
            if (isFinalWaveEnded && totalEnemyCount <= 0)
            {
                EndGame();
            }
            else
            {
                // 시간이 끝나거나 적을 다처치했는데 마지막웨이브가 아니면 다음으로 넘겨
                NextWave();
            }
        }
        // 웨이브 경과 시간 업데이트
        // 현재 웨이브 스폰 시작
        StartSpawnWave(Time.deltaTime);
    }

    private void StartSpawnWave(float deltaTime)
    {
        // 현재 웨이브의 각 스폰포인트 처리
        foreach (var spawnPoint in currentWave)
        {
            // 타이머 업데이트
            spawnPoint.timer += deltaTime;
            // 현재 스폰된 적이 max 카운트와 동일하다면 건너뜀 
            // 스폰해야할 적 숫자가 0 이라면 건너뜀
            if (spawnPoint.currentSpawnEnemyCount >= spawnPoint.maxSpawnCount ||
            spawnPoint.spawnCount <= 0)
            {
                continue;
            }

            // 아직 스폰 시작 시간이 안 됐으면 기다림
            if (spawnPoint.timer >= spawnPoint.spawnStartDelayTime && !spawnPoint.isStart)
            {
                spawnPoint.isStart = true;
                spawnPoint.timer = 0f; // 타이머 초기화
            }

            // 스폰 딜레이 시간마다 적 소환
            if (spawnPoint.timer >= spawnPoint.spawnDelayTime && spawnPoint.isStart)
            {
                // 적 소환
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
        // 웨이브 경과 시간 초기화  
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
        // 남은 스폰포인트 정보 다음 웨이브로 이관/순회하면서 
        foreach (var currentPoint in currentWave)
        {   // 활성화 혹은 스폰된 적이나 소환해야할 적없으면 스킵 
            if (!currentPoint.isStart || (currentPoint.currentSpawnEnemyCount <= 0 && currentPoint.spawnCount <= 0))
                continue;

            // 다음 웨이브에서 이 위치의 포인트가 있는지 확인
            var naxtPoint = nextWave.Find(p => p.position == currentPoint.position);
            // 있으면 정보 이관
            if (naxtPoint != null)
            {
                naxtPoint.currentSpawnEnemyCount += currentPoint.currentSpawnEnemyCount;
                naxtPoint.spawnCount += currentPoint.spawnCount;
            }
            else
            {
                //전 웨이브 남은 데이터 -> 다음웨이브에 없으면 새로 만들어서 다 소환될때까지 유지시키기 (포인터 유지)
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
        // 비워주고 
        currentWave.Clear();
        // 다음 웨이브로 교체
        currentWave = nextWave;
        // 인덱스도 바꿔주고 
        currentWaveIndex = nextWaveIndex;
        //시간도 초기화
        waveElapsedTime = 0f;
    }

    private void EndGame()
    {
        // 여기에 게임 종료 로직 추가
    }
}
