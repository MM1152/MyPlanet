using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class WaveManager : MonoBehaviour
{
    public Wave currentWave = new Wave();
    //현재 웨이브 인덱스
    private int currentWaveIndex = 1;
    public int CurrentWaveIndex => currentWaveIndex;
     
    // 이브 시작 셋팅, 데이터 및 종료조건 셋팅 
    private void StartWave()
    {
        // 데이터 가져옴      
        var data = DataTableManager.WaveTable.GetData(CurrentWaveIndex);
        if (data != null)
        {
            //데이터 정보넘김 
            currentWave.Init(data, EndWave);         
            #if DEBUG_LOG  
            Debug.Log($"웨이브 {CurrentWaveIndex} 시작");
            #endif
        }
        else
        {         
            // 다음 웨이브가없음          
#if DEBUG_LOG
            Debug.Log("더 이상 웨이브가 없습니다");
            throw new System.Exception("No more waves available");      
#endif
        }
    }
    // 웨이브 종료 처리
    private void EndWave()
    {
#if DEBUG_LOG  
       Debug.Log($"웨이브 {CurrentWaveIndex} 종료");    
#endif
        currentWaveIndex++;
        // 다음 웨이브 시작
        StartWave();
    }

    private void Update()
    {
        // 엔터 누르면 웨이브 시작 테스트용
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {            
            #if DEBUG_LOG
            Debug.Log("웨이브 시작");
            #endif
            StartWave();
        }
        // 현재 웨이브 업데이트 호출
        if (currentWave != null)
        {
            currentWave.Update();
        }
    }
}
