using UnityEngine;
using UnityEngine.UI;

public class PresetWindowTutorial2 : Tutorial
{
    private Button backButton;
    private Button intercactionButton;

    private bool isFirstUpdate = false;

    public override void TutorialEnter()
    {
        isFirstUpdate = false;

        backButton = GameObject.FindWithTag(TagIds.BackButton).GetComponent<Button>();
        intercactionButton = GameObject.FindWithTag(TagIds.TutorialTaget).GetComponent<Button>();

        backButton.onClick.AddListener(OnClickBackButton);
        intercactionButton.onClick.AddListener(OnClickInteractionButton);

        manager.SetTextAreaPosition(2);

        string msg = "이 곳에서는 현재 프리셋에 저장할 행성을 선택할 수 있습니다.";
        SetTextWithAnimation(msg).Forget();
        Debug.Log("Start Tutorial Preset 2");
    }

    public override void TutorialExit()
    {
        backButton.onClick.RemoveListener(OnClickBackButton);
        intercactionButton.onClick.RemoveListener(OnClickInteractionButton);
        Debug.Log("Exit Tutorial Preset 2");
    }

    public override void TutorialUpdate()
    {
        if(!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            isFirstUpdate = true;
            string msg = "아래에서 행성의 정보를 확인할 수 있고\n선택 완료를 터치하면 타워 배치 화면으로 넘어갑니다.";
            SetTextWithAnimation(msg , backGroundRayCastAble : false).Forget();
            manager.SetTouchPlanelParent(intercactionButton.transform);
        }
    }

    private void OnClickBackButton()
    {
        manager.SetPrevTutorial();
    }

    private void OnClickInteractionButton()
    {
        manager.SetNextTutorial();
    }
}