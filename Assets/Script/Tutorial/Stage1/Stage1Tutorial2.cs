using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1Tutorial2 : Tutorial
{
    private WaveManager waveManager;

    public override void TutorialEnter()
    {
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag).GetComponent<WaveManager>();
        Variable.IsSpawnActive = false;
        string msg = "가상의 조이스틱을 조작해 방어위성을 움직일 수 있습니다.\n원거리 적이 발사하는 투사체는 방어위성으로 막을 수 있고, 근거리 적은 그대로 통과됩니다.";
        manager.SetTutorialBackGround(false);
        manager.SetTextAreaPosition(4);
        SetTextWithAnimation(msg, callback: () => Variable.IsSpawnActive = true , backGroundRayCastAble : false).Forget();
        WaitForNextStageAsync().Forget();
    }

    public override void TutorialExit()
    {

    }

    public override void TutorialUpdate()
    {
        
    }

    private async UniTaskVoid WaitForNextStageAsync()
    {
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex == 2 , cancellationToken : manager.TutorialCtr.Token);
        manager.SetNextTutorial();
    }
}