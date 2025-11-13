using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerTest : MonoBehaviour
{
    [SerializeField]
    private WaveManager waveManager;
    [SerializeField]
    private TowerManager towerManager;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI count;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI expText;

    private void Update()
    {
        waveText.text = $"Wave : {waveManager.CurrentWaveIndex}";
        count.text = $"Count : {waveManager.currentWave.EnemyCount} / {waveManager.currentWave.TotalCount}";
        timeText.text = $"Time : {waveManager.currentWave.WaveTime:F2}";
        expText.text = $"Exp : {towerManager.TotalExp}";
    }

}
