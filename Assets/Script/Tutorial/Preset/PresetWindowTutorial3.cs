using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PresetWindowTutorial3 : Tutorial
{
    private Button backButton;
    private string msg = null;

    private List<int> stringTableIds = new List<int>() { 6244, 6245, 6246, 6247 };
    private int stringTableIndex = 0;
    public override void TutorialEnter()
    {
        stringTableIndex = 0;

        backButton = GameObject.FindGameObjectWithTag(TagIds.BackButton).GetComponent<Button>();
        backButton.onClick.AddListener(OnClickBackButton);

        Canvas.ForceUpdateCanvases();

        manager.SetTextAreaPosition(3);

        msg = DataTableManager.StringTable.Get(stringTableIds[stringTableIndex++]);
        SetTextWithAnimation(msg, backGroundRayCastAble: false).Forget();

        msg = DataTableManager.StringTable.Get(stringTableIds[stringTableIndex++]);

        Debug.Log("Start Tutorial Preset 3");
    }
    public override void TutorialUpdate()
    {
        if(!string.IsNullOrEmpty(msg) && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            stringTableIndex++;

            if (stringTableIndex > stringTableIds.Count)
            {
                msg = null;
                manager.SetNextTutorial();
                return;
            }
            SetTextWithAnimation(msg, backGroundRayCastAble: true).Forget();
            

            if(stringTableIndex < stringTableIds.Count)
            {
                msg = DataTableManager.StringTable.Get(stringTableIds[stringTableIndex]);
            }
        }
    }  
    public override void TutorialExit()
    {
        manager.SetTutorialBackGround(true);
        backButton.onClick.RemoveListener(OnClickBackButton);

        FirebaseManager.Instance.UserData.isClearPresetTutorial = true;
        FirebaseManager.Instance.UserData.ClearPresetTutorial().Forget();
        Debug.Log("Exit Tutorial Preset 3");
    }
    private void OnClickBackButton()
    {
        manager.SetPrevTutorial();
    }
}