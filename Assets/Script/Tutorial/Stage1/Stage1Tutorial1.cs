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
        isFirstUpdate = false;

        mainGameSceneCanvas = GameObject.FindWithTag(TagIds.MainGameSceneCanvas).GetComponent<Canvas>();
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();

        string msg = "스테이지 입장 시\n공격형 타워 하나를 선택할 수 있습니다.\n활성화 시킬 타워를 선택하세요";
        SetTextWithAnimation(msg, false).Forget();
        Time.timeScale = 0f;
    }

    public override void TutorialExit()
    {
        selectButton.onClick.RemoveListener(OnClickSelectButton);

        Time.timeScale = 1f;
    }

    public override void TutorialUpdate()
    {
        if (!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            mainGameSceneCanvas.sortingOrder = 9999;
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
            isFirstUpdate = true;
        }
    }

    private void OnClickSelectButton()
    {
        manager.SetNextTutorial();
    }

    private void OnClickSelectTowerUI()
    {
        mainGameSceneCanvas.sortingOrder = 0;
        foreach (var towerUI in selectTowerUIs)
        {
            GameObject.Destroy(towerUI.GetToggle().GetComponent<TutorialUiEvents>());
        }
        selectButton.onClick.AddListener(OnClickSelectButton);
        manager.SetTouchPlanelParent(selectButton.transform);
    }
}