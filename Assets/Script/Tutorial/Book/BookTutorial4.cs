using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class BookTutorial4 : Tutorial
{
    private string[] msgs = new string[] {
        "다음은 타워 정보를 확인하겠습니다.\n타워 탭을 터치하세요.",
        "타워 탭의 타워 카드를 터치하면 해당 타워의 상세 정보페이지를 볼 수 있습니다.",
        "타워 탭 또한 마찬가지로\n타워의 정보, 승급을 상세페이지에서 관리할 수 있습니다.",
        "마지막으로 배치를 관리하는 프리셋 탭을 터치해보세요."
    };
    private WindowManager windowManager;
    private Button presetTabButton;
    private Button towerButton;
    private bool isFirstUpdate = false;
    private bool isSecondUpdate = false;
    private bool blockTutorial = false;
    public override void TutorialEnter()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitleBookWindow);
        if(window is TitleBookWindow bookWindow)
        {
            presetTabButton = bookWindow.PresetTabButton;
            towerButton = bookWindow.TowerTablButton;
        }
        manager.SetTutorialBackGround(true);

        Variable.IsTutorialActive = true;
        presetTabButton.interactable = false;

        towerButton.onClick.AddListener(OnClickTowerButton);
        presetTabButton.onClick.AddListener(OnClickPresetTabButton);

        SetTextWithAnimation(msgs[0] , backGroundRayCastAble : false).Forget();
        TouchPositionUpdate2();

        blockTutorial = false;
        isFirstUpdate = false;
        isSecondUpdate = false;
    }

    public override void TutorialExit()
    {
        Variable.IsTutorialActive = false;
    }

    public override void TutorialUpdate()
    {
        if(isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            SetTextWithAnimation(msgs[2], callback: () => isSecondUpdate = true).Forget();
            isFirstUpdate = false;
        }
        else if (isSecondUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            presetTabButton.interactable = true;
            SetTextWithAnimation(msgs[3] , backGroundRayCastAble : false , callback : TouchPositionUpdate).Forget();
            isSecondUpdate = false;
        }
    }

    private void TouchPositionUpdate()
    {
        manager.SetTouchPlanelParent(presetTabButton.transform);
        manager.SetTutorialBackGround(false);
    }

    private void TouchPositionUpdate2()
    {
        manager.SetTouchPlanelParent(towerButton.transform);
    }

    private void OnClickPresetTabButton()
    {
        presetTabButton.onClick.RemoveListener(OnClickPresetTabButton);
        manager.SetNextTutorial();
    }

    private void OnClickTowerButton()
    {
        towerButton.onClick.RemoveListener(OnClickTowerButton);
        SetTextWithAnimation(msgs[1] , callback : () => isFirstUpdate = true).Forget();
    }
}