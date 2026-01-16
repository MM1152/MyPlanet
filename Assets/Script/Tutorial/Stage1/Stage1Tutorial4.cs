using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Tutorial4 : Tutorial
{
    private TowerManager towerManager;
    private WindowManager windowManager;
    private WaveManager waveManager;

    private Button selectButton;
    private List<SelectTowerUI> towerUis;

    private bool isFirstUpdate = false;
    public override void TutorialEnter()
    {
        base.TutorialEnter();
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        towerManager = GameObject.FindWithTag(TagIds.TowerManagerTag).GetComponent<TowerManager>();
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag).GetComponent<WaveManager>();

        Variable.IsSpawnActive = false;

        towerManager.SetLevel(2);
        WaitForBossStageAsync().Forget();
    }

    public override void TutorialExit()
    {
        base.TutorialExit();
        selectButton.onClick.RemoveListener(OnClickSelectButton);
    }

    public override void TutorialUpdate()
    {
        if (!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            isFirstUpdate = true;
            manager.SetActiveTutorialTextArea(false);

            var window = windowManager.Open(WindowIds.PlaceTowerWindow);
            if (window is PlaceTowerWindow placeTowerWindow)
            {
                placeTowerWindow.TutorialOpen(2015);
                selectButton = placeTowerWindow.GetSelectButton();


                towerUis = placeTowerWindow.GetSelectTowerUIs();

                manager.SetTouchPlanelParent(towerUis[1].transform);
                foreach (var towerUi in towerUis)
                {
                    var uiEvent = towerUi.GetToggle().AddComponent<TutorialUiEvents>();
                    uiEvent.OnClickAction += OnClickSelectTowerUI;
                }
            }
        }
    }

    private void OnClickSelectButton()
    {
        Variable.IsSpawnActive = true;
    }

    private void OnClickSelectTowerUI()
    {
        foreach (var towerUi in towerUis)
        {
            GameObject.Destroy(towerUi.GetToggle().GetComponent<TutorialUiEvents>());
        }
        selectButton.onClick.AddListener(OnClickSelectButton);
        manager.SetTouchPlanelParent(selectButton.transform);
    }

    private async UniTaskVoid WaitForBossStageAsync()
    {
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex == waveManager.MaxWave , timing : PlayerLoopTiming.PreUpdate , cancellationToken : manager.TutorialCtr.Token);
        manager.SetNextTutorial();
    }
}