using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfomationWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button infomationButton;
    [SerializeField] private Button starUpgradeButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerTypeText;
    [SerializeField] private TextMeshProUGUI towerAttackTypeText;
    [SerializeField] private TextMeshProUGUI towerElementText;
    [SerializeField] private TextMeshProUGUI towerInfomationText;
    [SerializeField] private TextMeshProUGUI towerOptionText;
    [SerializeField] private TextMeshProUGUI towerPeiceCountText;
    [SerializeField] private TextMeshProUGUI towerUpgradeStat;
    [Header("Images")]
    [SerializeField] private Image towerImage;
    [SerializeField] private Image towerTypeImage;
    [SerializeField] private Image towerAttackTypeImage;
    [SerializeField] private Image towerElementImage;
    [SerializeField] private Image[] starImages;
    [Header("Sprite")]
    [SerializeField] private Sprite enableStar;
    [SerializeField] private Sprite disableStar;
    [Header("Ref")]
    [SerializeField] private GameObject infomationTab;
    [SerializeField] private GameObject starUpgradeTab;
    [SerializeField] private GameObject infomationButtonBackGround;
    [SerializeField] private GameObject starUpgradeBackGround;

    private TowerData.Data userTowerData;
    private TowerTable.Data towerTableData;

    private GameObject currentTab;
    private GameObject currentBackGround;

    public override void Close()
    {
        currentTab?.SetActive(false);
        currentBackGround?.SetActive(false);

        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TowerInfomationWindow;

        infomationTab.SetActive(false);
        starUpgradeTab.SetActive(false);
        infomationButtonBackGround.SetActive(false);
        starUpgradeBackGround.SetActive(false);


        closeButton.onClick.AddListener(() => { manager.Open(WindowIds.TitleBookWindow); });
        infomationButton.onClick.AddListener(() => OpenTab(infomationTab , infomationButtonBackGround));
        starUpgradeButton.onClick.AddListener(() => OpenTab(starUpgradeTab , starUpgradeBackGround));
    }

    public override void Open()
    {
        currentTab = infomationTab;
        currentBackGround = infomationButtonBackGround;

        currentTab.SetActive(true);
        currentBackGround.SetActive(true);

        base.Open();
    }

    public void SettingTowerData(TowerTable.Data towerData)
    {
        this.towerTableData = towerData;
        this.userTowerData = FirebaseManager.Instance.TowerData.Get(this.towerTableData.id);

        closeButton.onClick.AddListener(() => manager.Open(WindowIds.TitleBookWindow));

        towerNameText.text = towerData.Name;
        towerPeiceCountText.text = $"부품 갯수 {userTowerData.TowerPartCount}/연결해야 됌";

        towerImage.sprite = towerTableData.towerImage;

        UpdateStar(userTowerData.grade);
        UpdateOptionValue(userTowerData);
    }

    private void UpdateOptionValue(TowerData.Data userTowerData)
    {
        var currentOptionValue = DataTableManager.TowerRandomOptionValueTable.GetMaxPercent(userTowerData.TowerId , userTowerData.grade);
        var nextOptionValue = DataTableManager.TowerRandomOptionValueTable.GetMaxPercent(userTowerData.TowerId, userTowerData.grade + 1);

        if (nextOptionValue == -1)
        {
            towerOptionText.text = currentOptionValue + "%";
            return;
        }

        towerOptionText.text = $"{currentOptionValue}% -> {nextOptionValue}";
    }

    private void ResetStar()
    {
        for(int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = disableStar;
        }
    }

    private void UpdateStar(int starCount) 
    {
        ResetStar();

        for (int i = 0; i < starCount; i++)
        {
            starImages[i].sprite = enableStar;
        }
    }

    private void OpenTab(GameObject target , GameObject buttonBackGround)
    {
        currentTab?.SetActive(false);
        currentBackGround?.SetActive(false);

        currentTab = target;
        currentBackGround = buttonBackGround;

        currentTab.SetActive(true);
        currentBackGround.SetActive(true);
    }
}
