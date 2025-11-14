using UnityEngine;
using System.Collections.Generic;
using System;

public class Wave
{
    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    // 총 적 수
    private int totalEnemyCount;
    public int TotalCount => totalEnemyCount;
    // 처치된 적 수
    private int defeatedEnemies;
    // 경과 시간
    private float elapsedTime;
    public float WaveTime => elapsedTime;
    // 현재 웨이브의 남은 적 수
    public int EnemyCount => totalEnemyCount - defeatedEnemies;
    // 다음 웨이브 진행 가능 여부
    public bool IsComplete => completed;
    // 시간되기전 다음으로 진행가능여부 확인용
    private bool completed => defeatedEnemies >= totalEnemyCount || elapsedTime <= 0;
    private Action onWaveEnd;
    private WaveData.Data waveData;
    private EnemySpawnManager enemySpawnManager;

    // 화면 크기 설정
    private Rect screenRect;
    // 각 면별로 스포너 갯수
    private const int topCount = 3;
    private const int rightCount = 4;
    private const int bottomCount = 3;
    private const int leftCount = 4;
    private const float addDis = 0.5f;
    public void Init()
    {
        SpawnaerSetting();
        enemySpawnManager = GameObject.FindGameObjectWithTag(TagIds.EnemySpawnManager).GetComponent<EnemySpawnManager>();
        // 모든 스폰포인트 초기화
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.Init(enemySpawnManager);
        }
    }

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
        spawnPoints.Clear();
        var xTopRatio = screenRect.width / (topCount + 1);
        var xRightRatio = screenRect.height / (rightCount + 1);
        var xBottomRatio = screenRect.width / (bottomCount + 1);
        var xLeftRatio = screenRect.height / (leftCount + 1);
        var pointIndex = 0;
        //위 시계방향으로 추가  25
        for (int i = 0; i < topCount; i++)
        {
            spawnPoints.Add(new SpawnPoint());
            spawnPoints[pointIndex].spawnPosition = new Vector3(screenRect.xMin + xTopRatio * i, screenRect.yMax + addDis, 0f);
            pointIndex++;
        }
        //오른쪽 Y -- 20
        for (int i = 0; i < rightCount; i++)
        {
            spawnPoints.Add(new SpawnPoint());
            spawnPoints[pointIndex].spawnPosition = new Vector3(screenRect.xMax + addDis, screenRect.yMax - (xRightRatio * i), 0f);
            pointIndex++;
        }
        //아래 25 
        for (int i = 0; i < bottomCount; i++)
        {
            spawnPoints.Add(new SpawnPoint());
            spawnPoints[pointIndex].spawnPosition = new Vector3(screenRect.xMax - (xBottomRatio * i), screenRect.yMin - addDis, 0f);
            pointIndex++;
        }
        //왼쪽 20 
        for (int i = 0; i < leftCount; i++)
        {
            spawnPoints.Add(new SpawnPoint());
            spawnPoints[pointIndex].spawnPosition = new Vector3(screenRect.xMin - addDis, screenRect.yMin + (xLeftRatio * i), 0f);
            pointIndex++;
        }
    }

    // 현재 웨이브 단계에 관한 정보를 넘김 
    public void UpdateData(WaveData.Data data, Action endWaveAction)
    {
        //데이터 저장
        waveData = data;
        //총 적 수 할당
        totalEnemyCount = data.Count;
        //종료 이벤트 전달 
        onWaveEnd = endWaveAction;
        // 웨이브 타임 할당 
        elapsedTime = data.Time;
        // 웨이브 관련정보 또 넘겨줌, 여기서 스폰포인트별로 각 웨이브에 해당하는데이터 분배
        spawnPoints[waveData.SpawnPoint].waveQueue.Enqueue(waveData);
    }
    //적이 처치될 때마다 호출, 적죽을때 마다 호출
    public void EnemyDefeated()
    {
        defeatedEnemies++;
    }

    public void Update()
    {
        // 각 스폰포인트별 업데이트 호출
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.Update(Time.deltaTime);
        }
        if (elapsedTime > 0)
        {
            // 시간감소
            elapsedTime -= Time.deltaTime;
        }
        if (completed && onWaveEnd != null)
        {
            defeatedEnemies = 0; // 처치된 적 수 초기화
            var endAction = onWaveEnd;
            onWaveEnd = null;  // 먼저 null로 만들어서 재진입 방지
            endAction.Invoke();  // 그 다음 실행          
        }
    }
}









