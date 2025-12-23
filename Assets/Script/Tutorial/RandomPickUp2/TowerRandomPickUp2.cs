using UnityEngine;
using UnityEngine.UI;

public class TowerRandomPickUp2 : Tutorial
{
    private string[] msgs = new string[]
    {
        "타워는 배치 시 다른 다른 타워에 버프를 줄 수 있는\n기능이 있습니다. 버프 수치는 랜덤입니다.",
        "행성과 마찬가지로 부품을 모아 등급을 올릴 수 있습니다.\n타워 조각은 중복 타워를 뽑았을 때\n랜덤 버프 수치가 보유 타워보다 낮다면 얻을 수 있습니다.",
    };

    private WindowManager windowManager;
    private Button okButton;
    private bool isFirstUpdate = false;
    public override void TutorialEnter()
    {
        isFirstUpdate = false;

        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitlePickUpResultWindow);

        if(window is TitlePickUpResultWindow randomPickUpWindow)
        {
            okButton = randomPickUpWindow.TowerPickUpResult.OkButton;
            Debug.Log("Okbutton", okButton.gameObject);
            okButton.onClick.AddListener(OnClickOkButton);
        }
    }

    public override void TutorialExit()
    {
        okButton.onClick.RemoveListener(OnClickOkButton);
    }

    public override void TutorialUpdate()
    {
        if (!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            isFirstUpdate = true;
            var clip = GetCombineClip(2, 9, 2, 10);
            SetTextWithAnimation(msgs[1], backGroundRayCastAble : false).Forget();
        }
        else if (isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            manager.SetNextTutorial();
            manager.InitTutorial(TutorialStep.Book);
        }
    }

    private void OnClickOkButton()
    {
        var clip = GetClip(2, 8);
        manager.SetTutorialBackGround(true);
        SetTextWithAnimation(msgs[0] , clip).Forget();
    }
}