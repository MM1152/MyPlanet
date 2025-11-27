using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DebugEnemySpawnViewer : MonoBehaviour
{
#if UNITY_EDITOR
    public TextMeshProUGUI enemyIdText;
    public Image enemyImage;
    public EnemyData.Data enemyData;

    private Button button;
    private EnemySpawnManager enemySpawnManager;
    private DebugEnemySpawnPanel debugEnemySpawnPanel;
    private Rect screenRect;

    public void Init(EnemyData.Data enemyData , EnemySpawnManager enemySpawnManager , DebugEnemySpawnPanel debugEnemySpawnPanel)
    {
        this.enemyData = enemyData;
        this.enemySpawnManager = enemySpawnManager;
        this.debugEnemySpawnPanel = debugEnemySpawnPanel;

        UpdateText();

        screenRect = Screen.safeArea;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickToSpawn);
    }

    private void OnClickToSpawn()
    {
        var spawnEnemy = enemySpawnManager.SpawnEnemy(enemyData.ID);
        var spawnPosition = new Vector3(Random.Range(screenRect.xMin , screenRect.xMax) , screenRect.yMax , -Camera.main.transform.position.z);

        spawnEnemy[0].transform.position = Camera.main.ScreenToWorldPoint(spawnPosition);
        spawnEnemy[0].Initallized(enemyData);

        debugEnemySpawnPanel.UpdateInputFiled(enemyData);
    }

    private void UpdateText()
    {
        enemyIdText.text = enemyData.ID.ToString();
    }
#endif
}
