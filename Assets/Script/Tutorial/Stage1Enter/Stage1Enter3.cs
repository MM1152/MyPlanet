using UnityEngine;
using UnityEngine.UI;

public class Stage1Enter3 : Tutorial
{
    private WindowManager windowManager;
    private Button interactionButton;
    public override void TutorialEnter()
    {
        manager.SetTutorialBackGround(false);
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitlePresetWindow);
        if(window is TitlePresetWindow presetWindow)
        {
            interactionButton = presetWindow.GameStartButton;
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