using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine.UI;

[Serializable]
public abstract class Tutorial
{
    protected TutorialManager manager;
    protected StringBuilder sb = new StringBuilder();
    public void Init(TutorialManager manager)
    {
        this.manager = manager;
    }

    public abstract void TutorialEnter();
    public abstract void TutorialUpdate();
    public abstract void TutorialExit();

    public async UniTaskVoid SetTextWithAnimation(string msg , bool backGroundRayCastAble = true, bool canPlayNextTutorial = false , Action callback = null)
    {
        int stringPointer = 0;
        manager.SetActiveTutorialTextEndImage(false);
        manager.SetActiveTutorialTextArea(true);
        while (stringPointer < msg.Length)
        {
            sb.Append(msg[stringPointer++]);
            manager.tutorialText.text = sb.ToString();
            await UniTask.Delay(30 , ignoreTimeScale : true , cancellationToken : manager.TutorialCtr.Token);
        }

        manager.SetActiveTutorialTextEndImage(true);
        manager.SetTutorialBackGround(backGroundRayCastAble);
        manager.CanPlayNextTutorial = canPlayNextTutorial;
        callback?.Invoke();
        sb.Clear();
    }
}
