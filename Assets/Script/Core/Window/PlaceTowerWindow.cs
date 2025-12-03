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
    [SerializeField] private ConsumableManager consumableManager;

    [Header("타워와 소모품 선택지에서 나올 확률")]
    [SerializeField] private float towerSpawnPercent;
    [SerializeField] private float consumableSpawnPercent;

    private List<SelectTowerUI> selectTowerUIs = new List<SelectTowerUI>();
    private int selectTowerIndex = -1;


#if DEBUG_MODE
    public Button testButton;
    [Header("뽑고 싶은 타워 ID 값 넣기")]
    public int towerId;
    [Header("뽑고 싶은 소모품 ID 값 넣기")]
    public int consumableId;
#endif

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        
        for (int i = 0; i < selectTowerUIRoot.childCount; i++)
        {
            SelectTowerUI obj = selectTowerUIRoot.GetChild(i).GetComponent<SelectTowerUI>();
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
        if (towerData != null)
        {
            towerManager.PlaceTower(towerData);
        }
        else
        {
            var consumData = selectTowerUIs[selectTowerIndex].GetCosumaableData();
            consumableManager.SetConsumable(consumData);
        }
        manager.Close();
    }

    public override void TutorialTowerOpen1()
    {
        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        var towerCount = towerManager.GetAllTower().Count;
        for (int i = 0; i < towerCount; i++)
        {
            if (towerManager.Towers[i] == null) continue;
            var tower = towerManager.GetTower(i);
            selectTowerUIs[i].SetInteractive(true);
            selectTowerUIs[i].SetTowerData(tower);
        }

        selectTowerUIs[1].SetInteractive(false);
        selectTowerUIs[2].SetInteractive(false);
        
        Time.timeScale = 0f;
        base.TutorialTowerOpen1();
    }

    public override void TutorialTowerOpen2()
    {
        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        var towerCount = towerManager.GetAllTower().Count;
        for (int i = 0; i < towerCount; i++)
        {
            if (towerManager.Towers[i] == null) continue;
            var tower = towerManager.GetTower(i);
            selectTowerUIs[i].SetTowerData(tower);
            selectTowerUIs[i].SetInteractive(true);
        }

        selectTowerUIs[0].SetInteractive(false);
        selectTowerUIs[2].SetInteractive(false);

        Time.timeScale = 0f;
        base.TutorialTowerOpen1();
    }

    public override void TutorialTowerOpen3()
    {
        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        var towerCount = towerManager.GetAllTower().Count;
        for (int i = 0; i < towerCount; i++)
        {
            if (towerManager.Towers[i] == null) continue;
            var tower = towerManager.GetTower(i);
            selectTowerUIs[i].SetTowerData(tower);
            selectTowerUIs[i].SetInteractive(true);
        }

        selectTowerUIs[0].SetInteractive(false);
        selectTowerUIs[1].SetInteractive(false);

        Time.timeScale = 0f;
        base.TutorialTowerOpen1();
    }

    public override void TutorialTowerOpen4()
    {
        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        var towerCount = towerManager.GetAllTower().Count;
        for (int i = 0; i < towerCount; i++)
        {
            if (towerManager.Towers[i] == null) continue;
            var tower = towerManager.GetTower(i);
            selectTowerUIs[i].SetInteractive(true);
            selectTowerUIs[i].SetTowerData(tower);
        }

        selectTowerUIs[1].SetInteractive(false);
        selectTowerUIs[2].SetInteractive(false);
        Time.timeScale = 0f;
        base.TutorialTowerOpen1();
    }

    public override void TutorialTowerOpen5()
    {
        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        var towerCount = towerManager.GetAllTower().Count;
        for (int i = 0; i < towerCount; i++)
        {
            if (i == 0)
            {
                selectTowerUIs[i].SetConsumableData(consumableManager.GetData(0));
                selectTowerUIs[i].SetInteractive(true);
                continue;
            }
            if (towerManager.Towers[i] == null) continue;
            var tower = towerManager.GetTower(i);
            selectTowerUIs[i].SetTowerData(tower);
            selectTowerUIs[i].SetInteractive(true);
        }

        selectTowerUIs[1].SetInteractive(false);
        selectTowerUIs[2].SetInteractive(false);

        Time.timeScale = 0f;
        base.TutorialTowerOpen1();
    }


    public override void Open()
    {
        if (Variable.IsTutorialActive) return;
        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        for (int i = 0; i < selectTowerUICount; i++)
        {
            selectTowerUIs[i].SetInteractive(true);
            var percent = Random.Range(0f, 1f);
            if (towerManager.CurrentLevel == 1) percent = 0f;
            if (percent < towerSpawnPercent)
            {
                selectTowerUIs[i].SetTowerData(towerManager.GetRandomTower());
            }
            else
            {
                selectTowerUIs[i].SetConsumableData(consumableManager.GetRandomData());
            }
        }
        Time.timeScale = 0f;
        base.Open();
    }

    public override void Close()
    {
        for(int i = 0; i < selectTowerUIs.Count; i++)
        {
            selectTowerUIs[i].ResetOutline();
        }
        base.Close();
    }
}
