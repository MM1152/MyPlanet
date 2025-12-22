using Firebase.Database;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

public class TitleBookWindow : Window
{
    [Header("UserData")]
    [SerializeField] private TextMeshProUGUI userNickName;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Header("Buttons")]
    [SerializeField] private Button planetButton;
    [SerializeField] private Button towerButton;
    [SerializeField] private Button presetButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button battleButton;
    [SerializeField] private Button gatchaButton;

    [Header("Planet")]
    [SerializeField] private PlanetInfomation planetInfomation;
    [SerializeField] private Transform planetInfomationRoot;

    [Header("Tower")]
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;

    [Header("Books")]
    [SerializeField] private GameObject planetBook;
    [SerializeField] private GameObject towerBook;
    [SerializeField] private GameObject presetBook;

    [Header("References")]
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private PresetViewer presetViewer;
    [SerializeField] private Transform presetViewerRoot;
    [SerializeField] private GameObject planetButtonBackGround;
    [SerializeField] private GameObject towerButtonBackGround;
    [SerializeField] private GameObject presetButtonBackGround;

    private List<PlanetInfomation> planetInfomationList = new List<PlanetInfomation>();
    private List<TowerInfomation> towerInfomationList = new List<TowerInfomation>();

    private GameObject currentOpenBook;
    private GameObject currentBackGround;

    private List<PresetViewer> presetViewers = new List<PresetViewer>();
    private int currentSelectPresetIndex = -1;

    public List<PlanetInfomation> PlanetInfomationList => planetInfomationList;
    public Button PresetTabButton => presetButton;
    public Button TowerTablButton => towerButton;

    public override void Init(WindowManager manager)
    {
        base.Init(manager);

        windowId = (int)WindowIds.TitleBookWindow;
        InitPlanetInfoList();
        InitTowerInfoList();
        InitPresetList();

        planetBook.SetActive(false);
        towerBook.SetActive(false);
        presetBook.SetActive(false);

        planetButtonBackGround.SetActive(false);
        towerButtonBackGround.SetActive(false);
        presetButtonBackGround.SetActive(false);

        FirebaseManager.Instance.PresetData.OnChangePresetData += ChangePresetData;

        towerButton.onClick.AddListener(() => OnClickBookButton(towerBook, towerButtonBackGround));
        planetButton.onClick.AddListener(() => OnClickBookButton(planetBook, planetButtonBackGround));
        presetButton.onClick.AddListener(() => OnClickBookButton(presetBook , presetButtonBackGround));

        homeButton.onClick.AddListener(() =>
        {
            manager.Open(WindowIds.TitleMainWindow);
        });

        battleButton.onClick.AddListener(() =>
        {
            manager.Open(WindowIds.TitleStageSelectedWindow);
        });

        gatchaButton.onClick.AddListener(() =>
        {
            manager.Open(WindowIds.RandomPickUpWindow);
        });

        currentOpenBook = planetBook;
        currentBackGround = planetButtonBackGround;
    }

    private void OnClickBookButton(GameObject targetBook , GameObject targetBackGround)
    {
        if(currentOpenBook != null)
        {
            currentOpenBook.SetActive(false);
            currentBackGround.SetActive(false);
        }
        currentOpenBook = targetBook;
        currentBackGround = targetBackGround;

        currentOpenBook.SetActive(true);
        currentBackGround.SetActive(true);
    }
    

    public override void Open()
    {
        currentOpenBook.SetActive(true);
        currentBackGround.SetActive(true);
        userNickName.text = FirebaseManager.Instance.UserData.nickName;
        goldText.text = FirebaseManager.Instance.UserData.gold.ToString("N0");
        expText.text = FirebaseManager.Instance.UserData.exp.ToString("N0");
        diamondText.text = FirebaseManager.Instance.UserData.diamond.ToString("N0");

        base.Open();
    }

    public override void Close()
    {
        if(currentOpenBook != null)
        {
            currentOpenBook.SetActive(false);
            currentBackGround.SetActive(false);
        }
        base.Close();
    }

    private void InitPlanetInfoList()
    {
        var planetDatas = DataTableManager.PlanetTable.GetAllData();
        
        for(int i = 0; i < planetDatas.Count; i++)
        {
            var planetInfo = Instantiate(planetInfomation , planetInfomationRoot);
            planetInfomationList.Add(planetInfo);
            planetInfo.OnClickPlanet += OpenBookInfomationWindow;
            planetInfo.UpdateTexts(planetDatas[i]);
        }
    }

    private void InitTowerInfoList()
    {
        var towerDatas = DataTableManager.TowerTable.GetAll();

        for (int i = 0; i < towerDatas.Count; i++)
        {
            var towerInfo = Instantiate(towerInfomation, towerInfomationRoot);
            towerInfomationList.Add(towerInfo);
            towerInfo.Init(towerDatas[i].ID);

            if (!FirebaseManager.Instance.TowerData.Get(towerDatas[i].ID).Unlock)
            {
                towerInfo.gameObject.SetActive(false);
            }

            towerInfo.OnTab += OnTabTowerInfomation;

            var path = string.Format(DataBasePaths.TowerUnlockPathFormating , towerDatas[i].ID);
            FirebaseManager.Instance.Database.AddListner(path, towerInfo.OnUnlockValueChanged);
        }
    }

    private void InitPresetList()
    {
        UpdatePreset();
    }

    private void ChangeSelectPresetIndex(int changeIdx)
    {

        if (currentSelectPresetIndex != -1)
        {
            presetViewers[currentSelectPresetIndex].UpdateSelectButton(false);
        }
        currentSelectPresetIndex = changeIdx;
        presetViewers[currentSelectPresetIndex].UpdateSelectButton(true);
    }

    private void UpdatePreset()
    {
        for (int i = 0; i < presetViewers.Count; i++)
        {
            Destroy(presetViewers[i].gameObject);
        }
        presetViewers.Clear();

        for (int i = 0; i < FirebaseManager.Instance.PresetData.Count(); i++)
        {
            var presetViewer = Instantiate(this.presetViewer, presetViewerRoot);
            presetViewer.Init(FirebaseManager.Instance.PresetData.Get(i), i, manager, ChangeSelectPresetIndex);
            presetViewer.CurrentWindowId = (WindowIds)windowId;
            presetViewers.Add(presetViewer);
        }
    }

    private void ChangePresetData(int index)
    {
        Debug.Log("Preset ChangeData Call");
        var presetData = FirebaseManager.Instance.PresetData.Get(index);
        presetViewers[index].UpdatePreset(presetData);
    }

    private void OnDestroy()
    {
        FirebaseManager.Instance.PresetData.OnChangePresetData -= ChangePresetData;
    }

    private void OpenBookInfomationWindow(PlanetTable.Data planetData , PlanetInfomation planetInfo)
    {
        var window = manager.Open(WindowIds.TitleBookInfomationWindow);

        if(window is TitleBookInfomationWindow bookInfoWindow)
        {
            bookInfoWindow.UpdatePlanetData(planetData);
        }
    }

    private void OnTabTowerInfomation(TowerTable.Data towerData)
    {
        var window = windowManager.Open(WindowIds.TowerInfomationWindow);
        
        if(window is TowerInfomationWindow tower)
        {
            tower.SettingTowerData(towerData);
        }
    }

}
