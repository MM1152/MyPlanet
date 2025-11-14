using UnityEngine;
using System.Collections.Generic;

public class SpawnPoint
{
    public enum SpawnState
    {
        Idle,
        Spawning,
        Completed
    }

    // 각 들어오는 에너미 숫자 및 웨이브 데이타를 가지고있어야함 순서대로 나가야하고 
    public Queue<WaveData.Data> waveQueue = new Queue<WaveData.Data>();
    // 각 스폰포인트별 포지션값을 저장하고 가지고있어야 각 스폰포인트가 스폰위치값을 오브젝트에 적용가능
    public Vector3 spawnPosition;
    private WaveData.Data currentWaveData;
    // 딜레이 검사용 타이머
    private float delayTimer = 0f;
    // 소환된 적수 검사용
    private int enemyCount = 0;
    // 인터벌 검사용 타이머
    private float intervalTimer = 0f;
    // 현재 스폰 상태
    private SpawnState currentState = SpawnState.Idle;

    private EnemySpawnManager enemySpawnManager;

    public void Init(EnemySpawnManager enemySpawnMgr)
    {
        enemySpawnManager = enemySpawnMgr;
    }

    public void Update(float deltaTime)
    {
        //현재 스폰중이 아니고 큐에 데이터가 남아있다면 순회
        if (currentState == SpawnState.Idle && waveQueue.Count > 0)
        {
            // 다음 웨이브 데이터 꺼내기
            currentWaveData = waveQueue.Dequeue();
            delayTimer = 0f;
            enemyCount = 0;
            intervalTimer = 0f;
            // 데이타가 들어오고 상태가 idle이라면 변경
            currentState = SpawnState.Spawning;
        }
        else if (currentState == SpawnState.Completed && waveQueue.Count > 0)
        {
            currentState = SpawnState.Idle;
        }

        if (currentState == SpawnState.Spawning)
        {
            // 스폰 딜레이 타이머 증가
            delayTimer += deltaTime;
            if (delayTimer >= currentWaveData.SpawnDelay)
            {
                // 스폰 인터벌 타이머 증가
                intervalTimer += deltaTime;
                if (intervalTimer >= currentWaveData.SpawnInterval)
                {
                    // 적 소환
                    var enemy = enemySpawnManager.SpawnEnemy(currentWaveData.EnemyID);

                    if (enemy != null)
                    {
                        enemy.transform.position = spawnPosition;                       
                        enemyCount++;
                        intervalTimer = 0f;

                        // 모든 적을 소환했는지 확인
                        if (enemyCount >= currentWaveData.Count)
                        {
                            currentState = SpawnState.Completed;
                        }
                    }
                }
            }
        }
    }


}
