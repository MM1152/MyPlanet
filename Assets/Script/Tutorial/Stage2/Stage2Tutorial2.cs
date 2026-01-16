using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Tutorial2 : Tutorial
{
    private WindowManager windowManager;

    public override void TutorialEnter()
    {
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        var window = windowManager.GetWindow(WindowIds.WarringWindow);

        if (window is WarringWindow warringWindow)
        {
            warringWindow.closeEvent += CloseEvent;
        }
    }

    private void CloseEvent()
    {
        base.TutorialEnter();

        var window = windowManager.GetWindow(WindowIds.WarringWindow);

        if (window is WarringWindow warringWindow)
        {
            warringWindow.closeEvent -= CloseEvent;
        }

        FirebaseManager.Instance.UserData.isClearStage2Tutorial = true;
        FirebaseManager.Instance.UserData.SaveAsync(DataBasePaths.UserPath + FirebaseManager.Instance.UserId , FirebaseManager.Instance.UserData).Forget();
    }
}