using UnityEngine;
using UnityEngine.UI;

public class TowerRandomPickUp1 : Tutorial
{
    private string msg = "다음은 새로운 타워입니다.\n1회 개발을 눌러보세요";

    private WindowManager windowManager;
    private PopupManager popupManager;

    private Button pickoneButton;
    private Button towerPickUpButton;
    private Button blueButton;

    public override void TutorialEnter()
    {
        manager.SetTextAreaPosition(1);
        manager.SetTutorialBackGround(false);
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        popupManager = GameObject.FindWithTag(TagIds.PopupManager).GetComponent<PopupManager>();

        var window = windowManager.GetWindow(WindowIds.RandomPickUpWindow);
        if(window is RandomPickUpWindow randomPickUpWindow)
        {
            pickoneButton = randomPickUpWindow.RandomPickUpLayoutForTower.PickOneButton;
            towerPickUpButton = randomPickUpWindow.TowerPickUpButton;
        }

        var popup = popupManager.GetPopup(PopupIds.TextPopup);
        if(popup is TextPopup textPopup)
        {
            blueButton = textPopup.BlueButton;
        }

        manager.SetTouchPlanelParent(towerPickUpButton.transform);
        towerPickUpButton.onClick.AddListener(OnClickTowerPickUpButton);
        pickoneButton.onClick.AddListener(OnClickPickUpButton);
    }

    public override void TutorialExit()
    {
        towerPickUpButton.onClick.RemoveListener(OnClickTowerPickUpButton);
        pickoneButton.onClick.RemoveListener(OnClickPickUpButton);
    }

    public override void TutorialUpdate()
    {

    }

    private void OnClickTowerPickUpButton()
    {
        manager.SetTutorialBackGround(true);
        manager.SetTouchPlanelParent(pickoneButton.transform);
        SetTextWithAnimation(msg, false).Forget();
    }

    private void OnClickPickUpButton()
    {
        manager.SetActiveTutorialTextArea(false);
        blueButton.onClick.AddListener(OnClickBlueButton);
        manager.SetTouchPlanelParent(blueButton.transform);
    }

    private void OnClickBlueButton()
    {
        manager.SetNextTutorial();
    }
}