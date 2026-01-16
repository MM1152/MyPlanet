using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Bson;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Tutorial : Tutorial
{
    private bool isFirstUpdate = false;
    private bool isSecondUpdate = false;
    private Image terraFormingHighlightImage;

    public override void TutorialEnter()
    {
        base.TutorialEnter();
        Variable.IsTutorialActive = false;

        terraFormingHighlightImage = GameObject.FindWithTag(TagIds.WaveWindowTag).GetComponent<WaveWindow>().TerraformingHighlightImage;
        terraFormingHighlightImage.gameObject.SetActive(true);

        //windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        //var window = windowManager.GetWindow(WindowIds.WarringWindow);

        //if(window is WarringWindow warringWindow)
        //{
        //    warringWindow.closeEvent += CloseEvent;
        //}
    }

    public override void TutorialExit()
    {
        base.TutorialExit();
        terraFormingHighlightImage.gameObject.SetActive(false);
    }

    //private void CloseEvent()
    //{
    //    var clip = GetCombineClip(3, 2, 3, 3);
    //    SetTextWithAnimation(msg[1], clip, backGroundRayCastAble: false).Forget();

    //    var window = windowManager.GetWindow(WindowIds.WarringWindow);

    //    if (window is WarringWindow warringWindow)
    //    {
    //        warringWindow.closeEvent -= CloseEvent;
    //    }

    //    isSecondUpdate = true;
    //}
}