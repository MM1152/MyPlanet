using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlaceTowerWindow : Window
{
    [SerializeField] private int selectTowerUICount;
    [SerializeField] private SelectTowerUI selectTowerUI;
    [SerializeField] private Transform selectTowerUIRoot;
    [SerializeField] private PlaceTower placeTower;
    [SerializeField] private TowerManager towerManager;
    
    private List<SelectTowerUI> selectTowerUIs = new List<SelectTowerUI>();
    public Button testButton;
    private TowerTable towerData;
    private PresetData presetData;

#if DEBUG_MODE
    [Header("뽑고 싶은 타워 ID 값 넣기")]
    public int towerId;
#endif

    public override void Init(WindowManager manager)
    {
        presetData = new PresetData(new List<int>());
        base.Init(manager);
        towerData = new TowerTable();
        for (int i = 0; i < selectTowerUICount; i++)
        {
            SelectTowerUI obj = Instantiate(selectTowerUI, selectTowerUIRoot);
            selectTowerUIs.Add(obj);
            Button objButton = obj.GetComponent<Button>();
            objButton.onClick.AddListener(() =>
            {
                towerManager.PlaceTower(obj.GetTowerData());
                placeTower.Place(obj.GetTowerData());
                manager.Close();
            });
        }
        testButton.onClick.AddListener(() => manager.Open(WindowIds.PlaceTowerWindow));
        windowId = (int)WindowIds.PlaceTowerWindow;

        
    }

    public async override void Open()
    {
        await DataTableManager.WaitForInitalizeAsync();
        await placeTower.Init();

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
