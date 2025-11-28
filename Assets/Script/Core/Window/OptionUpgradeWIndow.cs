using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OptionUpgradeWindow : Window
{
    [Header("Reference")]
    [SerializeField] private SelectOptionUI selectOptionUI;
    [SerializeField] private Transform selectOptionUIRoot;
    [SerializeField] private TowerManager towerManager;

    [Header("Buttons")]
    [SerializeField] private Button selectPlacePositionButton;
    [SerializeField] private Button selectButton;

    private List<SelectOptionUI> selectOptionUIs = new List<SelectOptionUI>();
    private int selectIndex = -1;
    public override void Close()
    {
        Time.timeScale = 1f;
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.OptionUpgradeWindow;

        for(int i = 0; i < 3; i++)
        {
            var optionUi = Instantiate(selectOptionUI, selectOptionUIRoot);
            int index = i;
            optionUi.Initalized(index, (idx) => selectIndex = idx);
            selectOptionUIs.Add(optionUi);
        }

        selectButton.onClick.AddListener(OnClickSelectButton);
    }

    public override void Open()
    {
        base.Open();
        Time.timeScale = 0f;
        for(int i = 0; i < selectOptionUIs.Count; i++)
        {
            var towerData = towerManager.GetRandomTower();
            selectOptionUIs[i].ResetOutline();
            selectOptionUIs[i].SetTowerData(towerData);
        }

        selectIndex = -1;
    }

    private void OnClickSelectButton()
    {
        if (selectIndex == -1) return;
        var towerData = selectOptionUIs[selectIndex].GetTowerData();

        towerData.Option.ResetRandomOption();

        var bonusAmount = 0;
        if(towerData.Option.GetOptionData().id == 1)
        {
            bonusAmount = DataTableManager.OptionTable.GetValueDataToInt(5014);
        }
        else if (towerData.Option.GetOptionData().id == 2)
        {
            bonusAmount = DataTableManager.OptionTable.GetValueDataToInt(5015);
        }
        else
        {
            bonusAmount = DataTableManager.OptionTable.GetValueDataToInt(5016);
        }

        towerData.Option.AddBonusOptionValue(bonusAmount);
        towerData.Option.SetRandomOption();

        manager.Close();
    }
}
