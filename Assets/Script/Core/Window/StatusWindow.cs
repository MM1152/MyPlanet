using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusWindow : Window
{
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private StatusViewer statusViewer;
    [SerializeField] private Transform statusViewerRoot;
    [SerializeField] private BasePlanet basePlanet;
    [SerializeField] private Button closeButton;

    private List<StatusViewer> statusViewers = new List<StatusViewer>();
    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.StatusWindow;

        var planetViewer = Instantiate(statusViewer, statusViewerRoot).GetComponent<StatusViewer>();
        planetViewer.Init(null, basePlanet);
        statusViewers.Add(planetViewer);

        for (int i = 0; i < towerManager.Towers.Count; i++)
        {
            if (towerManager.Towers[i] == null)
            {
                statusViewers.Add(null);
                continue;
            }  

            var statusView = Instantiate(statusViewer, statusViewerRoot).GetComponent<StatusViewer>();
            statusView.Init(towerManager.Towers[i] , basePlanet);
            statusViewers.Add(statusView);
        }

        closeButton.onClick.AddListener(() =>
        {
            manager.Close();
        });
    }

    public override void Open()
    {
        base.Open();
        statusViewers[0].UpdatePlanetText();
        for(int i = 1; i < towerManager.Towers.Count + 1; i++)
        {
            if (statusViewers[i] == null) continue;
            statusViewers[i].UpdateStatus();
        }
    }

    public override void Close()
    {
        base.Close();
    }
}
