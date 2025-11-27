using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TitleBookWindow : Window
{
    [Header("UserData")]
    [SerializeField] private TextMeshProUGUI userNickName;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;

    [Header("Buttons")]
    [SerializeField] private Button planetButton;
    [SerializeField] private Button towerButton;
    [SerializeField] private Button presetButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button battleButton;

    [Header("Planet")]
    [SerializeField] private PlanetInfomation planetInfomation;
    [SerializeField] private Transform planetInfomationRoot;

    [Header("Tower")]
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;

    [Header("Books")]
    [SerializeField] private GameObject planetBook;
    [SerializeField] private GameObject towerBook;

    private List<PlanetInfomation> planetInfomationList = new List<PlanetInfomation>();
    private List<TowerInfomation> towerInfomationList = new List<TowerInfomation>();
    private GameObject currentOpenBook;

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleBookWindow;
        InitPlanetInfoList();
        InitTowerInfoList();

        planetBook.SetActive(false);
        towerBook.SetActive(false);

        towerButton.onClick.AddListener(() =>
        {
            if(currentOpenBook != null)
            {
                currentOpenBook.SetActive(false);
            }

            currentOpenBook = towerBook;
            currentOpenBook.SetActive(true);
        });

        planetButton.onClick.AddListener(() =>
        {
            if (currentOpenBook != null)
            {
                currentOpenBook.SetActive(false);
            }

            currentOpenBook = planetBook;
            currentOpenBook.SetActive(true);
        });

        homeButton.onClick.AddListener(() =>
        {
            manager.Open(WindowIds.TitleMainWindow);
        });

        battleButton.onClick.AddListener(() =>
        {
            manager.Open(WindowIds.TitleStageSelectedWindow);
        });
    }

    public override void Open()
    {
        currentOpenBook = planetBook;
        currentOpenBook.SetActive(true);

        userNickName.text = FirebaseManager.Instance.UserData.nickName;
        goldText.text = FirebaseManager.Instance.UserData.gold.ToString();
        expText.text = FirebaseManager.Instance.UserData.exp.ToString();

        base.Open();
    }

    public override void Close()
    {
        if(currentOpenBook != null)
        {
            currentOpenBook.SetActive(false);
            currentOpenBook = null;
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
        }
    }

    private void OpenBookInfomationWindow(PlanetTable.Data planetData , PlanetInfomation planetInfo)
    {
        var window = manager.Open(WindowIds.TitleBookInfomationWindow);

        if(window is TitleBookInfomationWindow bookInfoWindow)
        {
            bookInfoWindow.UpdatePlanetData(planetData);
        }
    }

    private void UpdatePlanetInfoList()
    {

    }

    private void UpdateTowerInfoList()
    {

    }
}
