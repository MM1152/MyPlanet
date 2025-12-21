using UnityEngine;
using UnityEngine.UI;

public class BookTutorial1 : Tutorial
{
    private WindowManager windowManager;
    private Button bookButton;

    private string msg = "방금 얻은 행성과 타워의 정보가 저장되었습니다.\n정보는 도감 탭에서 확인할 수 있습니다.";

    public override void TutorialEnter()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.RandomPickUpWindow);

        if(window is RandomPickUpWindow randomPickUpWindow)
        {
            bookButton = randomPickUpWindow.BookButton;
            bookButton.onClick.AddListener(OnClickBookButton);
        }
        manager.SetTextAreaPosition(2);
        manager.SetTutorialBackGround(true);
        manager.SetTouchPlanelParent(bookButton.transform);
        SetTextWithAnimation(msg , backGroundRayCastAble : false).Forget();
    }

    public override void TutorialExit()
    {

    }

    public override void TutorialUpdate()
    {
    }

    private void OnClickBookButton()
    {
        bookButton.onClick.RemoveListener(OnClickBookButton);
        manager.SetNextTutorial();
    }
}