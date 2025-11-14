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

    private async UniTaskVoid Start()
    {
        await DataTableManager.WaitForInitalizeAsync();       
        currentWave.Init();
        StartWave();
    }

    private void StartWave()
    {
        // 데이터 가져옴      
        var data = DataTableManager.WaveTable.GetData(CurrentWaveIndex);
        if (data != null)
        {
            //데이터 정보넘김 
            currentWave.UpdateData(data, EndWave);                                   
       }
        else
        {        
            currentWaveIndex = 1;
            StartWave();            
        }
    }
    // 웨이브 종료 처리
    private void EndWave()
    {       
        currentWaveIndex++;
        // 다음 웨이브 시작
        StartWave();
    }

    private void Update()
    {
        // 현재 웨이브 업데이트 호출
        if (currentWave != null)
        {
            currentWave.Update();
        }
    }
}
