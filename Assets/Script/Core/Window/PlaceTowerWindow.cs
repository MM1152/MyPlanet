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

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        towerData = new TowerTable();
        for (int i = 0; i < selectTowerUICount; i++)
        {
            SelectTowerUI obj = Instantiate(selectTowerUI, selectTowerUIRoot);
            selectTowerUIs.Add(obj);
            Button objButton = obj.GetComponent<Button>();
            objButton.onClick.AddListener(() =>
            {
                if (!placeTower.Place())
                    return;

                towerManager.AddTower(obj.GetTowerData());
                manager.Close();
            });
        }
        testButton.onClick.AddListener(() => manager.Open(WindowIds.PlaceTowerWindow));
        windowId = (int)WindowIds.PlaceTowerWindow;
    }

    public async override void Open()
    {
        await DataTableManager.WaitForInitalizeAsync();

        for (int i = 0; i < selectTowerUICount; i++)
        {
            // FIX : 이부분 랜덤하게 데이터 넘겨주게 변경
            selectTowerUIs[i].SetTowerData(DataTableManager.Get<TowerTable>(DataTableIds.TowerTable).Get(i + 1));
        }

        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }
}
