using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BookTutorial3 : Tutorial
{
    private string msg = "상세페이지에서는 행성의 정보 확인은 물론\n행성의 레벨 업, 승급 진행을 할 수 있습니다.";

    private WindowManager windowManager;
    private Button exitButton;
    public override void TutorialEnter()
    {
        Variable.IsTutorialActive = true;
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitleBookInfomationWindow);

        if(window is TitleBookInfomationWindow infomation)
        {
            exitButton = infomation.ExitButton;
            exitButton.onClick.AddListener(OnClickExitButton);
            manager.SetTouchPlanelParent(exitButton.transform);
        }
            
        manager.SetTutorialBackGround(true);
        var clip = GetClip(2,15);
        SetTextWithAnimation(msg , clip : clip , backGroundRayCastAble : false).Forget();
    }

    public override void TutorialExit()
    {
    }

    public override void TutorialUpdate()
    {
    }

    public void OnClickExitButton()
    {
        exitButton.onClick.RemoveListener(OnClickExitButton);
        manager.SetNextTutorial();
        manager.InitTutorial(TutorialStep.Book1);
    }
}