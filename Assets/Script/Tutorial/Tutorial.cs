using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

[Serializable]
public class Tutorial
{
    protected TutorialManager manager;
    public bool isPlayTexts;
    protected StringBuilder sb = new StringBuilder();
    protected TutorialTable.Data tutorialData;
    public void Init(TutorialManager manager , TutorialTable.Data tutorialData)
    {
        this.manager = manager;
        this.tutorialData = tutorialData;
    }

    public virtual void TutorialEnter()
    {
        Debug.Log($"Start Tutorial {tutorialData.ID}");
        Canvas.ForceUpdateCanvases();

       // manager.SetTouchPlanelParent(interactionButton[0].transform);
        manager.SetTextAreaPosition(tutorialData.TutorialAreaPosition);
        manager.SetTouchPlanelParent(manager.transform);
        manager.SetActiveTouchPanel(false);

        if (tutorialData.TargetButtonID != -1)
        {
            var interactionButton = manager.GetInteractionButton(tutorialData.TargetButtonID);
            interactionButton.UpdateButton();
            if(interactionButton != null)
            {
                manager.SetTouchPlanelParent(interactionButton.transform);
                manager.SetActiveTouchPanel(true);
            }
        }

        var clip = GetCombineClip(new (int, int)[] {
            (tutorialData.ClipType , tutorialData.Clip1),
            (tutorialData.ClipType , tutorialData.Clip2),
            (tutorialData.ClipType , tutorialData.Clip3),
        });

        Time.timeScale = tutorialData.TimeScale;

        if (string.IsNullOrEmpty(tutorialData.TutorialText))
        {
            manager.SetTutorialBackGround(tutorialData.BackGroundLayoutRayCast);
            manager.CanPlayNextTutorial = tutorialData.CanNextPlay;
        }
        else
        {
            SetTextWithAnimation(
                tutorialData.TutorialText,
                clip,
                tutorialData.BackGroundLayoutRayCast,
                tutorialData.CanNextPlay
            ).Forget();
        }
    }

    public virtual void TutorialUpdate()
    {

    }

    public virtual void TutorialExit()
    {
        manager.SetActiveTutorialTextArea(false);
        manager.SetTouchPlanelParent(manager.transform);
    }

    public async UniTaskVoid SetTextWithAnimation(string msg , AudioClip clip = null, bool backGroundRayCastAble = true, bool canPlayNextTutorial = false , Action callback = null)
    {
        manager.Skip = false;
        isPlayTexts = true;
        int stringPointer = 0;
        if(clip != null)
        {
            manager.PlaySound(clip);
        }
        manager.SetActiveTutorialTextEndImage(false);
        manager.SetActiveTutorialTextArea(true);
        while (stringPointer < msg.Length)
        {
            sb.Append(msg[stringPointer++]);
            manager.tutorialText.text = sb.ToString();

            if (manager.Skip)
            {
                manager.tutorialText.text = msg;
                manager.Skip = false;
                break;
            }

            await UniTask.Delay(30 , ignoreTimeScale : true , cancellationToken : manager.TutorialCtr.Token);
        }

        manager.SetActiveTutorialTextEndImage(true);
        manager.SetTutorialBackGround(backGroundRayCastAble);
        manager.CanPlayNextTutorial = canPlayNextTutorial;
        callback?.Invoke();
        callback = null;
        isPlayTexts = false;
        sb.Clear();
    }

    protected AudioClip GetClip(int type , int id)
    {
        return DataTableManager.SoundsTable.Get(type, id);
    }

    protected AudioClip GetCombineClip(int type, int id , int type1, int id1)
    {
        var clip1 = DataTableManager.SoundsTable.Get(type, id);
        var clip2 = DataTableManager.SoundsTable.Get(type1, id1);
        var combineClip = Utils.CombineAudioClips(clip1, clip2);
        return combineClip;
    }

    protected AudioClip GetCombineClip(params (int type, int id)[] ids)
    {
        List<AudioClip> audioClips = new List<AudioClip>();
        foreach (var tuple in ids)
        {
            var clipData = DataTableManager.SoundsTable.Get(tuple.type, tuple.id);
            if(clipData != null)
            {
                audioClips.Add(clipData);
            }
        }

        return Utils.CombineMultipleAudioClips(audioClips.ToArray());
    }
}
