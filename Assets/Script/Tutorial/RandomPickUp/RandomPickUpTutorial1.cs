using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class  RandomPickUpTutorial1 : Tutorial
{
    private string msg = "플레이하고 얻은 재화를 통해 새로운 행성과 타워를 뽑을 수 있습니다.\n뽑기 탭을 선택하세요";
    private WindowManager windowManager;
    private Button gachaTabButton;
    public override void TutorialEnter()
    {
        Variable.IsTutorialActive = false;

        manager.SetTextAreaPosition(1);
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitleMainWindow);
        if(window is TitleMainWindow titleMainWindow)
        {
            gachaTabButton = titleMainWindow.GachaButton;
            gachaTabButton.onClick.AddListener(OnClickGachaTabButton);
        }

        var clip = DataTableManager.SoundsTable.Get(2, 0);
        var clip1 = DataTableManager.SoundsTable.Get(2, 1);

        var combineClip = Utils.CombineAudioClips(clip, clip1);
        manager.SetTouchPlanelParent(gachaTabButton.transform);

        SetTextWithAnimation(msg, combineClip,  backGroundRayCastAble: false).Forget();
    }

    public override void TutorialExit()
    {
        gachaTabButton.onClick.RemoveListener(OnClickGachaTabButton);
    }

    public override void TutorialUpdate()
    {

    }

    private void OnClickGachaTabButton()
    {
        manager.SetNextTutorial();
    }
}
