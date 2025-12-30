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
    private List<Tower> availableTowers = new List<Tower>();
    private int selectIndex = -1;
    public override void Close()
    {
        base.Close();
    }
    [Header("Sprites")]
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.OptionUpgradeWindow;

        for (int i = 0; i < selectOptionUIRoot.childCount; i++)
        {
            var optionUi = selectOptionUIRoot.GetChild(i).GetComponentInChildren<SelectOptionUI>();
            int index = i;
            optionUi.Initalized(index, (idx) => selectIndex = idx);
            selectOptionUIs.Add(optionUi);
        }

        selectButton.onClick.AddListener(OnClickSelectButton);
    }

    public override void Open()
    {
        if (Variable.IsTutorialActive) return;
        base.Open();
        Time.timeScale = 0f;

        availableTowers.Clear();
        var allTowers = towerManager.GetAllTower();
        for (int i = 0; i < allTowers.Count; i++)
        {
            if (allTowers[i] != null)
            {
                availableTowers.Add(allTowers[i]);
            }
        }
        var fillCount = Mathf.Min(selectOptionUIs.Count, availableTowers.Count);
        for (int i = 0; i < fillCount; i++)
        {

            int randomIndex = Random.Range(0, availableTowers.Count);
            var towerData = availableTowers[randomIndex];
            availableTowers.RemoveAt(randomIndex);

            selectOptionUIs[i].gameObject.SetActive(true);
            selectOptionUIs[i].SetInteractive(true);

            var lefticonindex = towerData.SlotIndex/10;
            var righticonindex = towerData.SlotIndex % 10;

            selectOptionUIs[i].SetTowerData(towerData, GetIndexIconSprite(lefticonindex), GetIndexIconSprite(righticonindex));
        }
        for (int i = fillCount; i < selectOptionUIs.Count; i++)
        {
            selectOptionUIs[i].gameObject.SetActive(false);
            selectOptionUIs[i].SetInteractive(false);
        }

        selectIndex = -1;
    }

    private void OnClickSelectButton()
    {
        if (selectIndex == -1) return;
        var towerData = selectOptionUIs[selectIndex].GetTowerData();
        var bonusAmount = selectOptionUIs[selectIndex].GetBonusAmount();

        towerData.Option.ResetRandomOption();
        towerData.Option.AddBonusOptionValue(bonusAmount);
        towerData.Option.SetRandomOption();
        Debug.Log($"선택 보너스{bonusAmount}% 적용 완료");
        manager.Close();
    }

   

    public Sprite GetIndexIconSprite(int index)
    {
        if (index < 0 || index >= sprites.Count) return null;
        return sprites[index];
    }
}
