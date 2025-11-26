using UnityEngine;
using UnityEngine.UI;

public class DebugTowerPlaceWindow : Window
{
    public DebugPlaceViewer placeViewer;
    public DebugTowerManager towerManager;
    public Transform placeViewerRoot;
    public Button backButton;
    public override void Close()
    {
        base.Close();
        Variable.IsJoyStickActive = true;
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);

        var towers = towerManager.GetAllTower();
        for(int i = 0; i < towers.Count; i++) 
        {
            var placeview = Instantiate(placeViewer, placeViewerRoot);   
            placeview.Init(towerManager , towers[i]);
        }

        windowId = (int)WindowIds.DebugTowerPlaceWIndow;
        backButton.onClick.AddListener(() => manager.Close());
    }

    public override void Open()
    {
        base.Open();
        Variable.IsJoyStickActive = false;
    }

}
