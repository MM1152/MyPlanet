using Cysharp.Threading.Tasks;
using Firebase.Database;
using System.IO;
using System.Runtime.CompilerServices;
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

    [SerializeField] private TextMeshProUGUI firstStatusTitle;
    [SerializeField] private TextMeshProUGUI firstStatusValue;
    [SerializeField] private TextMeshProUGUI secondStatusTitle;
    [SerializeField] private TextMeshProUGUI secondStatusValue;
    [SerializeField] private TextMeshProUGUI thirdStatusTitle;
    [SerializeField] private TextMeshProUGUI thirdStatusValue;

    [SerializeField] private TextMeshProUGUI randomOptionText;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI userNameText;

    [Header("Images")]
    [SerializeField] private Image towerImage;
    [SerializeField] private Image towerTypeImage;
    [SerializeField] private Image towerAttackTypeImage;
    [SerializeField] private Image towerElementImage;
    [SerializeField] private Image[] starImages;
    [Header("Sprite")]
    [SerializeField] private Sprite enableStar;
    [SerializeField] private Sprite disableStar;
    [Header("Slider")]
    [SerializeField] private Slider pieceSlider;
    [Header("Ref")]
    [SerializeField] private GameObject infomationTab;
    [SerializeField] private GameObject starUpgradeTab;
    [SerializeField] private GameObject infomationButtonBackGround;
    [SerializeField] private GameObject starUpgradeBackGround;
    [SerializeField] private GameObject typeLayout;
    [SerializeField] private GameObject attacktypeLayout;
    [SerializeField] private GameObject elementLayout;


    private TowerData.Data userTowerData;
    private TowerTable.Data towerTableData;

    private GameObject currentTab;
    private GameObject currentBackGround;

    public override void Close()
    {
        currentTab?.SetActive(false);
        currentBackGround?.SetActive(false);

        if(towerTableData != null)
        {
            var path = string.Format(DataBasePaths.TowerGradeFormating, towerTableData.ID);
            FirebaseManager.Instance.Database.RemoveListner(path, OnValueChangeGrade);
        }

        base.Close();
    }

    private void OnDestroy()
    {
        if (towerTableData == null) return;

        var path = string.Format(DataBasePaths.TowerGradeFormating, towerTableData.ID);
        FirebaseManager.Instance.Database.RemoveListner(path, OnValueChangeGrade);
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
        upgradeButton.onClick.AddListener(() => UpgradeStar().Forget());

        userNameText.text = FirebaseManager.Instance.UserData.nickName;

        goldText.text = FirebaseManager.Instance.UserData.gold.ToString();
        diamondText.text = FirebaseManager.Instance.UserData.diamond.ToString();
        expText.text = FirebaseManager.Instance.UserData.exp.ToString();

        FirebaseManager.Instance.Database.AddListner(DataBasePaths.GoldPath, OnChangeGoldValue);
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.ExpPath, OnChangeExpValue);
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.DiamondPath, OnChangeDiamondValue);
    }

    public override void Open()
    {
        currentTab = infomationTab;
        currentBackGround = infomationButtonBackGround;

        currentTab.SetActive(true);
        currentBackGround.SetActive(true);

        elementLayout.SetActive(true);
        typeLayout.SetActive(true);
        attacktypeLayout.SetActive(true);

        base.Open();
    }

    public void SettingTowerData(TowerTable.Data towerData)
    {
        this.towerTableData = towerData;
        this.userTowerData = FirebaseManager.Instance.TowerData.Get(this.towerTableData.ID);

        closeButton.onClick.AddListener(() => manager.Open(WindowIds.TitleBookWindow));

        var path = string.Format(DataBasePaths.TowerGradeFormating, towerData.ID);
        FirebaseManager.Instance.Database.AddListner(path, OnValueChangeGrade);

        var pieceCount = DataTableManager.TowerGradeToPieceCountTable.GetPieceCount(towerData.ID , userTowerData.grade);

        towerNameText.text = towerData.Name;

        towerImage.sprite = towerTableData.towerImage;
        towerImage.preserveAspect = true;

        if (towerTableData.ElementImage == null)
            elementLayout.SetActive(false);
        if(towerTableData.TypeImage == null)
            typeLayout.SetActive(false);
        if(towerTableData.AttackTypeImage == null)
            attacktypeLayout.SetActive(false);


        towerElementImage.sprite = towerTableData.ElementImage;
        towerTypeImage.sprite = towerTableData.TypeImage;
        towerAttackTypeImage.sprite = towerTableData.AttackTypeImage;

        towerElementText.text = towerTableData.AttributeToString;
        towerTypeText.text = towerTableData.TypeToString;
        towerAttackTypeText.text = towerTableData.AttackTypeToString;
        towerInfomationText.text = towerTableData.Explanatoin;

        var randomOptionData = RandomOptionData.GetData(towerData.Option);
        var randomOption = randomOptionData.option.DeepCopy();
        randomOption.Init(towerData, randomOptionData);

        randomOptionText.text = string.Format(randomOption.FormatingString);
        randomOptionText.text += " " + randomOption.GetOptionStringFormatting();
        randomOptionText.text += $" <color=yellow>{userTowerData.OptionValue} %</color>";
        if (towerData.Type == 1)
            UpdateTextToAttackTypeTower(towerData);
        else
            UpdateTextToUtilTypeTower(towerData as TowerTable.UtilTower);
        UpdateStar(userTowerData.grade);
        UpdateOptionValue(userTowerData);
        UpdateSlider(userTowerData.grade);
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

        towerOptionText.text = $"{currentOptionValue}% -> {nextOptionValue}%";
    }

    private void OnValueChangeGrade(object sender , ValueChangedEventArgs args)
    {
        var starCount = int.Parse(args.Snapshot.Value.ToString());
        UpdateStar(starCount);
        UpdateSlider(starCount);
    }

    private void UpdateSlider(int grade)
    {
        var pieceCount = DataTableManager.TowerGradeToPieceCountTable.GetPieceCount(towerTableData.ID, grade);
        var curPieceCount = userTowerData.TowerPartCount;

        pieceSlider.value =  (float)curPieceCount / pieceCount;
        towerPeiceCountText.text = $"부품 갯수 {curPieceCount}/{pieceCount}";
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

    private void UpdateTextToAttackTypeTower(TowerTable.Data tower)
    {
        firstStatusTitle.text = "공격력[ATK]";
        firstStatusValue.text = "+" + tower.ATK.ToString("F2");

        secondStatusTitle.text = "연사 속도[FIRE_RATE]";
        secondStatusValue.text = "+" + tower.Fire_Rate.ToString("F2");

        thirdStatusTitle.text = "사거리[RANGE]";
        thirdStatusValue.text = "+" + tower.Attack_Range.ToString("F2");
    }

    private void UpdateTextToUtilTypeTower(TowerTable.UtilTower tower)
    {
        firstStatusTitle.text = "지속시간[DURATION]";
        firstStatusValue.text = "+" + tower.Duration.ToString("F2");

        secondStatusTitle.text = "쿨타임[COOLTIME]";
        secondStatusValue.text = "+" + tower.Cooltime.ToString("F2");

        thirdStatusTitle.text = "범위[RANGE]";
        thirdStatusValue.text = "+" + tower.range.ToString("F2");
    }

    private void OnChangeGoldValue(object sender, ValueChangedEventArgs args)
    {
        goldText.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

    private void OnChangeDiamondValue(object sender, ValueChangedEventArgs args)
    {
        diamondText.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

    private void OnChangeExpValue(object sender, ValueChangedEventArgs args)
    {
        expText.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

    private async UniTaskVoid UpgradeStar()
    {
        var pieceCount = DataTableManager.TowerGradeToPieceCountTable.GetPieceCount(towerTableData.ID, userTowerData.grade);

        if (pieceCount > userTowerData.TowerPartCount || userTowerData.grade == 5) return;

        await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.TowerData.UpgradeGrade(userTowerData.TowerId , pieceCount));
    }
}
