using UnityEngine;
using TMPro;

public class WaveUITest : MonoBehaviour
{
   public TextMeshProUGUI waveText;
   public TextMeshProUGUI EnemyCountText;
   public TextMeshProUGUI TimerText;

   public TextMeshProUGUI WaveClearCountText;
    
   public WaveManager waveManager;
    
    private void Update()
    {
        if(waveManager.UIUpdateTest)
        {
         waveText.text = $"Wave : {waveManager.CurrentWaveIndex+1} "; 
         EnemyCountText.text = $"Count : {waveManager.totalEnemyCount} ";
         TimerText.text = $"Next :{waveManager.WaveDuration:00} \nTime :{waveManager.WaveElapsedTime:00}";
         WaveClearCountText.text = $"Clear : {waveManager.waveClearCount}";
        }
    }  
}
