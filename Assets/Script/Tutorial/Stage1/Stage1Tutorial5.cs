using UnityEditor;
using UnityEngine;

public class Stage1Tutorial5 : Tutorial
{
    private WindowManager windowManager;
    private WarringWindow warningWindow;

    private bool isFirstUpdate = false;
    public override void TutorialEnter()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        warningWindow = windowManager.GetWindow(WindowIds.WarringWindow) as WarringWindow;

        warningWindow.closeEvent += OnCloseEvent;
        isFirstUpdate = false;
    }

    public override void TutorialExit()
    {
        base.TutorialExit();
        Variable.IsSpawnActive = true;
    }

    public override void TutorialUpdate()
    {
        if (!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            manager.SetNextTutorial();
        }
    }

    private void OnCloseEvent()
    {
        base.TutorialEnter();
        warningWindow.closeEvent -= OnCloseEvent;
        Time.timeScale = 0f;
    }
}