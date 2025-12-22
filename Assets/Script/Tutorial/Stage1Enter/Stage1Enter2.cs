using UnityEngine;
using UnityEngine.UI;

public class Stage1Enter2 : Tutorial
{
    private WindowManager windowManager;
    private Button interactionButton;
    public override void TutorialEnter()
    {
        manager.SetTutorialBackGround(false);
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitleStageSelectedWindow);
        if(window is TitleStageSelectWindow stageSelectedWindow)
        {
            interactionButton = stageSelectedWindow.SelectButton;
            interactionButton.onClick.AddListener(OnClickInteractionButton);

            manager.SetTouchPlanelParent(interactionButton.transform);
        }
    }
    public override void TutorialExit()
    {
        interactionButton.onClick.RemoveListener(OnClickInteractionButton);
    }
    public override void TutorialUpdate()
    {
        
    }
    private void OnClickInteractionButton()
    {
        manager.SetNextTutorial();
    }

}