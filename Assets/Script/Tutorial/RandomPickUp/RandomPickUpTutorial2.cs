using UnityEngine;
using UnityEngine.UI;

public class RandomPickUpTutorial2 : Tutorial
{
    private string msg = "우선 새로운 행성을 찾아봅시다.\n1회 탐사를 선택하세요\n탐사 비용을 지원해드리겠습니다.";
    private Button pickOneButton;
    private Button blueButton;

    private PopupManager popupManager;
    public override void TutorialEnter()
    {
        manager.SetTutorialBackGround(true);
        pickOneButton = GameObject.FindWithTag(TagIds.TutorialTaget).GetComponent<Button>();
        popupManager = GameObject.FindWithTag(TagIds.PopupManager).GetComponent<PopupManager>();
        var popup = popupManager.GetPopup(PopupIds.TextPopup);
        if(popup is TextPopup textPopup)
        {
            blueButton = textPopup.BlueButton;
        }

        pickOneButton.onClick.AddListener(OnClickPickOneButton);

        manager.SetTextAreaPosition(1);
        manager.SetTouchPlanelParent(pickOneButton.transform);
        SetTextWithAnimation(msg, backGroundRayCastAble: false).Forget();
    }

    public override void TutorialExit()
    {
        pickOneButton.onClick.RemoveListener(OnClickPickOneButton);
    }

    public override void TutorialUpdate()
    {

    }

    private void OnClickPickOneButton()
    {
        manager.SetTouchPlanelParent(blueButton.transform);
        manager.SetActiveTutorialTextArea(false);
        blueButton.onClick.AddListener(OnClickBlueButton);
    }

    private void OnClickBlueButton()
    {
        manager.SetNextTutorial();
    }
}