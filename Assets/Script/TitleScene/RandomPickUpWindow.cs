using UnityEngine;
using UnityEngine.UI;

public class RandomPickUpWindow : Window
{
    [SerializeField] private RandomPickUpLayout randomPickUpLayoutForPlanet;
    [SerializeField] private RandomPickUpLayout randomPickUpLayoutForTower;
    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button planetPickUpButton;
    [SerializeField] private Button towerPickUpButton;
    [Header("Ref")]
    [SerializeField] private GameObject planetPickUpPanel;
    [SerializeField] private GameObject towerPickUpPanel;

    private GameObject previousOpenPanel;

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
        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));

        previousOpenPanel = planetPickUpPanel;
        planetPickUpPanel.SetActive(true);

        planetPickUpButton.onClick.AddListener(OnClickPlanetPickUpButton);
        towerPickUpButton.onClick.AddListener(OnClickTowerPickUpButton);
    }

    public override void Open()
    {
        if (previousOpenPanel != null)
        {
            previousOpenPanel.SetActive(true);
        }
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
}
