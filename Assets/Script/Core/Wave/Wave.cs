using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;

public class Wave
{
    private int totalEnemyCount;
    public int TotalCount => totalEnemyCount;

    private int defeatedEnemies;
    
    // 경과 시간
    private float elapsedTime;
    public float WaveTime => elapsedTime;   
    
    // 현재 웨이브의 남은 적 수
    public int EnemyCount => totalEnemyCount - defeatedEnemies;
    // 다음 웨이브 진행 가능 여부
    public bool IsComplete => completed;
    // 시간되기전 다음으로 진행가능여부 확인용
    private bool completed => defeatedEnemies >= totalEnemyCount;
    private Action onWaveEnd;
    private WaveData.Data waveData;
    private EnemySpawnManager enemySpawnManager;    

    // 호옥시 특정웨이브가 종료됐을때 다른게 실행될수있음    
    public void Init(WaveData.Data data, Action endWaveAction)
    {
        waveData = data;    
        totalEnemyCount += data.Count;
        elapsedTime += data.Time;
        onWaveEnd = endWaveAction;
        enemySpawnManager = GameObject.FindGameObjectWithTag(TagIds.EnemySpawnManager).GetComponent<EnemySpawnManager>();    
        StartSpawning().Forget();
    } 
   
    //적이 처치될 때마다 호출, 적죽을때 마다 호출
    public void EnemyDefeated()
    {
        defeatedEnemies++;
    }

    public void Update()
    {
        if (elapsedTime > 0)
        {        
            // 시간감소
            elapsedTime = Mathf.Max(0, elapsedTime - Time.deltaTime);
            #if DEBUG_LOG  
            Debug.Log($"Elapsed Time: {elapsedTime}");
            #endif
            if (elapsedTime <= 0 || completed)
            {        
                #if DEBUG_LOG  
                Debug.Log("Wave completed.");        
                #endif
                // 다음웨이브로 넘겨줄수있게 처리
                onWaveEnd?.Invoke();
            }            
        }
    }

    private async UniTaskVoid StartSpawning()
    {
       await SpawnEnemies();    
    }

    private async UniTask SpawnEnemies()
    {
        if (waveData.SpawnDelay > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(waveData.SpawnDelay));
        }
        for(int i =0; i< waveData.Count; i++)
        {    
          enemySpawnManager.SpawnEnemy(waveData.EnemyID , waveData.SpawnPoint);           
            //다음 소환까지 대기
            if (waveData.SpawnInterval > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(waveData.SpawnInterval));
            }
        }     
    }
}








