using UnityEditor;
using UnityEngine;

public class Stage1Tutorial5 : Tutorial
{
    private string msg = "마지막 웨이브에서는 보스가 등장합니다\n다른 적들과 달리 강하며 패턴을 가지고 있고\n보스를 처치하면 스테이지를 클리어하게 됩니다";

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
        Variable.IsSpawnActive = true;
        Time.timeScale = 1f;
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
        var clip1 = DataTableManager.SoundsTable.Get(1, 7);
        var clip2 = DataTableManager.SoundsTable.Get(1, 8);
        var clip3 = DataTableManager.SoundsTable.Get(1, 9);

        var combineClip = Utils.CombineMultipleAudioClips(new AudioClip[] { clip1, clip2, clip3 });
        SetTextWithAnimation(msg , combineClip).Forget();
        warningWindow.closeEvent -= OnCloseEvent;
        Time.timeScale = 0f;
    }
}