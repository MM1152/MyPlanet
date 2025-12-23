using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class Tutorial
{
    protected TutorialManager manager;
    public bool isPlayTexts;
    protected StringBuilder sb = new StringBuilder();
    public void Init(TutorialManager manager)
    {
        this.manager = manager;
    }

    public abstract void TutorialEnter();
    public abstract void TutorialUpdate();
    public abstract void TutorialExit();

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
            audioClips.Add(DataTableManager.SoundsTable.Get(tuple.type, tuple.id));
        }

        return Utils.CombineMultipleAudioClips(audioClips.ToArray());
    }
}
