using UnityEngine;

public class BookTutorial2 : Tutorial
{
    private string[] msgs = new string[]
    {
        "도감에서는 행성과 타워 정보, 프리셋을 관리 할 수 있습니다.",
        "행성 탭의 행성 카드를 터치하면 해당 행성의 상세 정보페이지를 볼 수 있습니다."
    };

    private WindowManager windowManager;
    private bool isFirstUpdate = false;
    public override void TutorialEnter()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();

        var window = windowManager.GetWindow(WindowIds.TitleBookWindow);
        if (window is TitleBookWindow titleBookWindow)
        {
            foreach(var viewer in titleBookWindow.PlanetInfomationList)
            {
                viewer.OnClickPlanet += OnClickPlanet;
            }
        }

        manager.SetTextAreaPosition(3);
        var clip = GetClip(2,12);
        SetTextWithAnimation(msgs[0], clip, backGroundRayCastAble : false).Forget();   
    }

    public override void TutorialExit()
    {
        var window = windowManager.GetWindow(WindowIds.TitleBookWindow);
        if (window is TitleBookWindow titleBookWindow)
        {
            foreach (var viewer in titleBookWindow.PlanetInfomationList)
            {
                viewer.OnClickPlanet -= OnClickPlanet;
            }
        }
    }

    public override void TutorialUpdate()
    {
        if(!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            isFirstUpdate = true;
            var clip = GetClip(2, 14);
            SetTextWithAnimation(msgs[1],  clip, backGroundRayCastAble : true, callback: EndTextAniamtionCallBack).Forget();
        }
    }

    private void EndTextAniamtionCallBack()
    {
        Variable.IsTutorialActive = false;

        var window = windowManager.GetWindow(WindowIds.TitleBookWindow);
        if (window is TitleBookWindow titleBookWindow)
        {
            manager.SetTouchPlanelParent(titleBookWindow.PlanetInfomationList[0].transform);
        }

    }

    public void OnClickPlanet(PlanetTable.Data data, PlanetInfomation info)
    {
        manager.SetNextTutorial();
    }
}