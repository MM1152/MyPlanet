using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class  RandomPickUpTutorial1 : Tutorial
{
    private string msg = "플레이하고 얻은 재화를 통해 새로운 행성과 타워를 뽑을 수 있습니다.\n가챠 탭을 선택하세요";
    private Button gachaTabButton;
    public override void TutorialEnter()
    {
        Variable.IsTutorialActive = false;

        manager.SetTextAreaPosition(1);

        gachaTabButton = GameObject.FindWithTag(TagIds.TutorialTaget).GetComponent<Button>();
        gachaTabButton.onClick.AddListener(OnClickGachaTabButton);
        
        manager.SetTouchPlanelParent(gachaTabButton.transform);

        SetTextWithAnimation(msg, backGroundRayCastAble: false).Forget();
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
