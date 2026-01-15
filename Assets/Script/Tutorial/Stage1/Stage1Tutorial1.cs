using Cysharp.Threading.Tasks.Triggers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Tutorial1 : Tutorial
{
    private WindowManager windowManager;
    private Button selectButton;
    private List<SelectTowerUI> selectTowerUIs = new List<SelectTowerUI>();
    private Canvas mainGameSceneCanvas;
    private bool isFirstUpdate = false;

    public override void TutorialEnter()
    {
        base.TutorialEnter();
        isFirstUpdate = false;
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        Time.timeScale = 0f;
    }

    public override void TutorialExit()
    {
        base.TutorialExit();
        selectButton.onClick.RemoveListener(OnClickSelectButton);
        Time.timeScale = 1f;
    }

    public override void TutorialUpdate()
    {
        if (!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            //mainGameSceneCanvas.sortingOrder = 9999;
            manager.SetActiveTutorialTextArea(false);
            manager.SetTutorialBackGround(false);

            var window = windowManager.Open(WindowIds.PlaceTowerWindow);
            if (window is PlaceTowerWindow placeTowerWindow)
            {

                placeTowerWindow.TutorialOpen(2003);
                selectButton = placeTowerWindow.GetSelectButton();

                selectTowerUIs = placeTowerWindow.GetSelectTowerUIs();
                for (int i = 0; i < selectTowerUIs.Count; i++)
                {
                    var tutorialUIEvenets = selectTowerUIs[i].GetToggle().AddComponent<TutorialUiEvents>();
                    tutorialUIEvenets.OnClickAction += OnClickSelectTowerUI;
                }

                manager.SetTouchPlanelParent(selectTowerUIs[1].transform);
            }
            manager.SetActiveTutorialTextArea(false);
            isFirstUpdate = true;
        }
    }

    private void OnClickSelectButton()
    {
        manager.SetNextTutorial();
    }

    private void OnClickSelectTowerUI()
    {
        //mainGameSceneCanvas.sortingOrder = 0;
        foreach (var towerUI in selectTowerUIs)
        {
            GameObject.Destroy(towerUI.GetToggle().GetComponent<TutorialUiEvents>());
        }
        selectButton.onClick.AddListener(OnClickSelectButton);
        manager.SetTouchPlanelParent(selectButton.transform);
    }
}