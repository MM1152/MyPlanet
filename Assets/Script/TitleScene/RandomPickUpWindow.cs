using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomPickUpWindow : Window
{
    [SerializeField] private RandomPickUpLayout randomPickUpLayoutForPlanet;
    [SerializeField] private RandomPickUpLayout randomPickUpLayoutForTower;
    [Header("Buttons")]
    [SerializeField] private Button planetPickUpButton;
    [SerializeField] private Button towerPickUpButton;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button battleButton;
    [SerializeField] private Button bookButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI userNickName;
    [Header("Images")]
    [SerializeField] private Image userIconImage;

    [Header("Ref")]
    [SerializeField] private GameObject planetPickUpPanel;
    [SerializeField] private GameObject towerPickUpPanel;

    private GameObject previousOpenPanel;

    public RandomPickUpLayout RandomPickUpLayoutForTower => randomPickUpLayoutForTower;
    public Button TowerPickUpButton => towerPickUpButton;
    public Button BookButton => bookButton;
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.RandomPickUpWindow;

        planetPickUpPanel.SetActive(false);
        towerPickUpPanel.SetActive(false);

        randomPickUpLayoutForPlanet.Init(DataTableManager.RandomPickUpTable.GetAllDataForPlanet());
        randomPickUpLayoutForTower.Init(DataTableManager.RandomPickUpTable.GetRandomPickUpDatasForTower());

        previousOpenPanel = planetPickUpPanel;
        planetPickUpPanel.SetActive(true);

        planetPickUpButton.onClick.AddListener(OnClickPlanetPickUpButton);
        towerPickUpButton.onClick.AddListener(OnClickTowerPickUpButton);

        homeButton.onClick.AddListener(OnClickHomeButton);
        battleButton.onClick.AddListener(OnClickBattleButton);
        bookButton.onClick.AddListener(OnClickBookButton);

        FirebaseManager.Instance.Database.AddListner(DataBasePaths.DiamondPath, OnValueChangedDiamond);
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.GoldPath, OnValueChangedGold);
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.ExpPath, OnValueChangedExp);

        diamondText.text = FirebaseManager.Instance.UserData.diamond.ToString();
        goldText.text = FirebaseManager.Instance.UserData.gold.ToString();
        expText.text = FirebaseManager.Instance.UserData.exp.ToString();

        userNickName.text = FirebaseManager.Instance.Auth.UserDisplayName;
        userIconImage.sprite = FirebaseManager.Instance.Auth.UserIconSprite;
    }

    public override void Open()
    {
        if (previousOpenPanel != null)
        {
            previousOpenPanel.SetActive(true);
        }
        Managers.SoundManager.PlayBGM(AudiosId.Dry_To_Verb);
        base.Open();
    }

    private void OnClickPlanetPickUpButton()
    {
        previousOpenPanel?.SetActive(false);
        previousOpenPanel = planetPickUpPanel;
        previousOpenPanel.SetActive(true);
    }

    private void OnClickTowerPickUpButton()
    {
        previousOpenPanel?.SetActive(false);
        previousOpenPanel = towerPickUpPanel;
        previousOpenPanel.SetActive(true);
    }

    private void OnValueChangedDiamond(object sender , ValueChangedEventArgs args)
    {
        diamondText.text = args.Snapshot.Value.ToString();
    }

    private void OnValueChangedGold(object sender, ValueChangedEventArgs args)
    {
        goldText.text = args.Snapshot.Value.ToString();
    }

    private void OnValueChangedExp(object sender, ValueChangedEventArgs args)
    {
        expText.text = args.Snapshot.Value.ToString();
    }

    private void OnClickHomeButton()
    {
        manager.Open(WindowIds.TitleMainWindow);
    }

    private void OnClickBattleButton()
    {
        manager.Open(WindowIds.TitleStageSelectedWindow);
    }

    private void OnClickBookButton()
    {
        manager.Open(WindowIds.TitleBookWindow);
    }
}
