using UnityEngine;
using UnityEngine.UI;

public class Stage1Enter1 : Tutorial
{
    private WindowManager windowManager;
    private string msg = "버턴트 프로토콜의 지휘관님 환영합니다.\n가장 기본적인 임무인 전투에 대해 알려드리겠습니다.\n하단의 전투 버튼을 터치하세요.";
    private Button interactionButton;
    public override void TutorialEnter()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitleMainWindow);
        if(window is TitleMainWindow mainWindow)
        {
            interactionButton = mainWindow.SelectStageButton;
            interactionButton.onClick.AddListener(OnClickInteractionButton);
        }
        SetTextWithAnimation(msg, false, callback : () => manager.SetTouchPlanelParent(interactionButton.transform)).Forget();
    }
    public override void TutorialExit()
    {
        interactionButton.onClick.RemoveListener(OnClickInteractionButton);
    }
    public override void TutorialUpdate()
    {

    }

    private void OnClickInteractionButton()
    {
        manager.SetNextTutorial();
    }
}