using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleBookInfomationWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button infomationButton;
    [SerializeField] private Button levelUpbutton;
    [SerializeField] private Button starUpgradeButton;

    [SerializeField] private PlanetInfoViewer planetInfoViewer;
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleBookInfomationWindow;
        exitButton.onClick.AddListener(() => {
            manager.Open(WindowIds.TitleBookWindow);
        });
    }

    public override void Open()
    {
        base.Open();
    }

    public void UpdatePlanetData(PlanetTable.Data planetTableData)
    {
        planetInfoViewer.UpdatePlanetData(planetTableData);
        planetInfoViewer.UpdateTexts();
    }

}
