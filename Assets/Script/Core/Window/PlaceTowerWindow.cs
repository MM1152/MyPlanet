using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlaceTowerWindow : Window
{
    [SerializeField] private int selectTowerUICount;

    [Header ("Drag To Inspector")]
    [SerializeField] private SelectTowerUI selectTowerUI;
    [SerializeField] private Transform selectTowerUIRoot;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button selectTowerButton;

    private List<SelectTowerUI> selectTowerUIs = new List<SelectTowerUI>();
    private int selectTowerIndex = -1;
#if DEBUG_MODE
    public Button testButton;
    [Header("뽑고 싶은 타워 ID 값 넣기")]
    public int towerId;
#endif

    public override void Init(WindowManager manager)
    {
        base.Init(manager);

        for (int i = 0; i < selectTowerUICount; i++)
        {
            SelectTowerUI obj = Instantiate(selectTowerUI, selectTowerUIRoot);
            obj.Initalized(i , (value) => selectTowerIndex = value);
            selectTowerUIs.Add(obj);
        }

#if DEBUG_MODE
        testButton.gameObject.SetActive(true);
        testButton.onClick.AddListener(() => manager.Open(WindowIds.PlaceTowerWindow));
#endif
        windowId = (int)WindowIds.PlaceTowerWindow;
        selectTowerButton.onClick.AddListener(OnClickSelectTowerButton);
    }

    private void OnClickSelectTowerButton()
    {
        if (selectTowerIndex == -1) return;

        var towerData = selectTowerUIs[selectTowerIndex].GetTowerData();
        towerManager.PlaceTower(towerData);
        manager.Close();
    }

    public override void Open()
    {
        if(selectTowerIndex != -1)
        {
            selectTowerUIs[selectTowerIndex].ResetOutline();
            selectTowerIndex = -1;
        }

        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        for (int i = 0; i < selectTowerUICount; i++)
        {
            var tower = towerManager.GetRandomTower();
#if DEBUG_MODE
            if (towerId != -1)
            {
                tower = towerManager.GetTower(towerId);
            }
#endif
            // FIX : 이부분 랜덤하게 데이터 넘겨주게 변경
            selectTowerUIs[i].SetTowerData(tower);
        }
        Time.timeScale = 0f;
        base.Open();
    }

    public override void Close()
    {
        Time.timeScale = 1f;
        base.Close();
    }
}
