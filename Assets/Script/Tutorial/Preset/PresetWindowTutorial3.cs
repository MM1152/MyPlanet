using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PresetWindowTutorial3 : Tutorial
{
    private Button backButton;
    private string msg = null;

    private List<int> stringTableIds = new List<int>() { 6244, 6245, 6246, 6247 };
    private List<Func<AudioClip>> audiooClips = new List<Func<AudioClip>>() 
    { 
        () => Utils.CombineMultipleAudioClips(new AudioClip[] { DataTableManager.SoundsTable.Get(4, 5), DataTableManager.SoundsTable.Get(4, 4), DataTableManager.SoundsTable.Get(4, 6) }),
        () => Utils.CombineAudioClips(DataTableManager.SoundsTable.Get(4, 7), DataTableManager.SoundsTable.Get(4, 8)),
        () => DataTableManager.SoundsTable.Get(4, 11),
        () => Utils.CombineAudioClips(DataTableManager.SoundsTable.Get(4, 9), DataTableManager.SoundsTable.Get(4, 10)),
    };
    private int stringTableIndex = 0;
    public override void TutorialEnter()
    {
        stringTableIndex = 0;

        backButton = GameObject.FindGameObjectWithTag(TagIds.BackButton).GetComponent<Button>();
        backButton.onClick.AddListener(OnClickBackButton);

        Canvas.ForceUpdateCanvases();

        manager.SetTextAreaPosition(3);

        msg = DataTableManager.StringTable.Get(stringTableIds[stringTableIndex]);
        SetTextWithAnimation(msg, audiooClips[stringTableIndex++]?.Invoke() , backGroundRayCastAble: false).Forget();

        Debug.Log("Start Tutorial Preset 3");
    }
    public override void TutorialUpdate()
    {
        if(!string.IsNullOrEmpty(msg) && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            stringTableIndex++;

            if (stringTableIndex >= stringTableIds.Count)
            {
                msg = null;
                manager.SetNextTutorial();
                return;
            }
            if (stringTableIndex < stringTableIds.Count)
            {
                msg = DataTableManager.StringTable.Get(stringTableIds[stringTableIndex]);
            }

            SetTextWithAnimation(msg, audiooClips[stringTableIndex]?.Invoke() , backGroundRayCastAble: true).Forget();
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