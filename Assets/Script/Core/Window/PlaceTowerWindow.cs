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

    public override void Open()
    {
        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        for (int i = 0; i < selectTowerUICount; i++)
        {
            var percent = Random.Range(0f, 1f);
            if(percent < towerSpawnPercent)
            {
                var tower = towerManager.GetRandomTower();
                selectTowerUIs[i].SetTowerData(tower);
            }
            else
            {
                var consumable = consumableManager.GetRandomData();
                selectTowerUIs[i].SetConsumableData(consumable);
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
        Time.timeScale = 1f;
        base.Close();
    }
}
