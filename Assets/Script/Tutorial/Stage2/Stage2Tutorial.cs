using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Bson;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Tutorial : Tutorial
{
    private bool isFirstUpdate = false;
    private bool isSecondUpdate = false;
    private Image terraFormingHighlightImage;
    private WindowManager windowManager;
    private string[] msg = new string[] {
        "테라포밍 게이지가 20%, 45%, 75%, 100%에\n도달할 때마다 패시브를 선택할 수 있습니다\n한 번 선택한 패시브는 스테이지가 종료될 때까지 변경할 수 없습니다",
        "스테이지를 진행하면서 일정 웨이브마다 등장하는 엘리트 몬스터입니다.\n엘리트 몬스터는 일반 적들보다 공격력과 체력이 높은 대신\n처치 시 타워 슬릇을 강화시킬 수 있습니다"
    };
    public override void TutorialEnter()
    {
        Variable.IsTutorialActive = false;

        manager.SetTutorialBackGround(false);
        manager.SetTextAreaPosition(3);
        terraFormingHighlightImage = GameObject.FindWithTag(TagIds.WaveWindowTag).GetComponent<WaveWindow>().TerraformingHighlightImage;
        terraFormingHighlightImage.gameObject.SetActive(true);

        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.WarringWindow);

        if(window is WarringWindow warringWindow)
        {
            warringWindow.closeEvent += CloseEvent;
        }

        var clip = GetCombineClip(3, 0, 3, 1);
        SetTextWithAnimation(msg[0] , clip, backGroundRayCastAble : false).Forget();
    }

    public override void TutorialExit()
    {
    }

    public override void TutorialUpdate()
    {
        if(!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            manager.SetActiveTutorialTextArea(false);
            terraFormingHighlightImage.gameObject.SetActive(false);
            isFirstUpdate = true;
        }

        if(isSecondUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            manager.SetNextTutorial();
            FirebaseManager.Instance.UserData.isClearStage2Tutorial = true;
            var path = DataBasePaths.UserPath + FirebaseManager.Instance.UserId;
            FirebaseManager.Instance.UserData.SaveAsync(path , FirebaseManager.Instance.UserData).Forget();
        }
    }

    private void CloseEvent()
    {
        var clip = GetCombineClip(3, 2, 3, 3);
        SetTextWithAnimation(msg[1], clip, backGroundRayCastAble: false).Forget();

        var window = windowManager.GetWindow(WindowIds.WarringWindow);

        if (window is WarringWindow warringWindow)
        {
            warringWindow.closeEvent -= CloseEvent;
        }

        isSecondUpdate = true;
    }
}