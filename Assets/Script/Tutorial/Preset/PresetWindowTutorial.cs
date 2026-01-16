using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetWindowTutorial : Tutorial
{
    private WindowManager windowManager;
    public override void TutorialEnter()
    {
        base.TutorialEnter();

        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.TitlePresetWindow);
        if(window is TitlePresetWindow presetWindow)
        {
            presetWindow.UpdateTutorialPart();
            var editButton = presetWindow.GetEditButtonForTutorial();
            editButton.onClick.AddListener(OnClickInteractionButton);
            manager.SetTouchPlanelParent(editButton.transform);
        }
    }

    public override void TutorialExit()
    {
        manager.SetTutorialBackGround(true);

        var window = windowManager.GetWindow(WindowIds.TitlePresetWindow);
        if (window is TitlePresetWindow presetWindow)
        {
            presetWindow.ReseteTutorialPart();
            var editButton = presetWindow.GetEditButtonForTutorial();
            editButton.onClick.RemoveListener(OnClickInteractionButton);
        }
    }

    private void OnClickInteractionButton()
    {
        manager.SetNextTutorial();
    }
}