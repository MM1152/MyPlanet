using UnityEngine;
using UnityEngine.UI;

public class RandomPickUpTutorial3 : Tutorial
{
    private string msg = "행성은 바로 전투에서 사용할 수 있으며\n행성 조각은 일정 갯수를 모아 완성된 행성을\n만들 수 있습니다.";
    private bool isFirstUpdate = false;
    private Button okButton;
    private WindowManager windowManager;
    public override void TutorialEnter()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitlePickUpResultWindow);
        if(window is TitlePickUpResultWindow pickupResultWindow)
        {
            okButton = pickupResultWindow.PlanetPickUpResult.OkButton;
        }
        isFirstUpdate = false;
        okButton.onClick.AddListener(OnClickOkButton);
    }

    public override void TutorialExit()
    {
        okButton.onClick.RemoveListener(OnClickOkButton);
    }

    public override void TutorialUpdate()
    {
        if (!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            manager.SetNextTutorial();
            manager.InitTutorial(TutorialStep.PickUp2);
        }
    }

    private void OnClickOkButton()
    {
        SetTextWithAnimation(msg).Forget();
        manager.SetTutorialBackGround(true);
    }
}