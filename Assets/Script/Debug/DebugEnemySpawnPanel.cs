using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
public class DebugEnemySpawnPanel : MonoBehaviour
{
#if UNITY_EDITOR
    public DebugEnemySpawnViewer enemySpawnViewer;
    public Transform enemySpawnViewerRoot;
    public EnemySpawnManager spawnManager;
    public Button saveButton;

    [Header("Debug ¿ë")]
    public TextMeshProUGUI idText;
    public TMP_InputField speedInput;
    public TMP_InputField hpInput;
    public TMP_InputField expInput;
    public TMP_InputField firerateInput;
    public TMP_InputField rangeInput;
    public TMP_InputField bulletSpeedInput;

    private EnemyData.Data currentEnemyData;
    private List<EnemyData.Data> enemyDatas;
    private void Start()
    {
        enemyDatas = DataTableManager.EnemyTable.GetAllDataDeepCopy();
        for( int i = 0; i < enemyDatas.Count; i++)
        {
            var viewer = Instantiate(enemySpawnViewer, enemySpawnViewerRoot);
            viewer.Init(enemyDatas[i], spawnManager , this);
        }

        speedInput.onValidateInput += ValidateNumericInput;
        hpInput.onValidateInput += ValidateNumericInput;
        expInput.onValidateInput += ValidateNumericInput;
        firerateInput.onValidateInput += ValidateNumericInput;
        rangeInput.onValidateInput += ValidateNumericInput;
        bulletSpeedInput.onValidateInput += ValidateNumericInput;

        speedInput.onEndEdit.AddListener((x) => UpdateEnemyData(currentEnemyData));
        hpInput.onEndEdit.AddListener((x) => UpdateEnemyData(currentEnemyData));
        expInput.onEndEdit.AddListener((x) => UpdateEnemyData(currentEnemyData));
        firerateInput.onEndEdit.AddListener((x) => UpdateEnemyData(currentEnemyData));
        rangeInput.onEndEdit.AddListener((x) => UpdateEnemyData(currentEnemyData));
        bulletSpeedInput.onEndEdit.AddListener((x) => UpdateEnemyData(currentEnemyData));

        saveButton.onClick.AddListener(() =>
        {
            saveButton.interactable = false;
            SaveEnemyData().Forget();
        });

        UpdateInputFiled(enemyDatas[0]);
    }

    public void UpdateInputFiled(EnemyData.Data enemyData)
    {
        idText.text = enemyData.ID.ToString();
        speedInput.text = enemyData.Speed.ToString();
        hpInput.text = enemyData.HP.ToString();
        expInput.text = enemyData.EXP.ToString();
        firerateInput.text = enemyData.Fire_Rate.ToString();
        rangeInput.text = enemyData.Range.ToString();
        bulletSpeedInput.text = enemyData.Bullet_Speed.ToString();

        this.currentEnemyData = enemyData;
    }

    public void UpdateEnemyData(EnemyData.Data enemyData)
    {
        enemyData.Speed = int.Parse(speedInput.text);
        enemyData.HP = int.Parse(hpInput.text);
        enemyData.EXP = int.Parse(expInput.text);
        enemyData.Fire_Rate = int.Parse(firerateInput.text);
        enemyData.Range = float.Parse(rangeInput.text);
        enemyData.Bullet_Speed = int.Parse(bulletSpeedInput.text);

    }

    public async UniTaskVoid SaveEnemyData()
    {
        await DataTableManager.EnemyTable.SaveData(DataTableIds.EnemyTable , enemyDatas);
        saveButton.interactable = true;
    }

    private char ValidateNumericInput(string text, int charIndex, char addedChar)
    {
        // Allow digits, and potentially a decimal point for float values
        if (char.IsDigit(addedChar) || (addedChar == '.' && !text.Contains(".")))
        {
            return addedChar;
        }
        return '\0'; // Disallow other characters
    }
#endif
}
