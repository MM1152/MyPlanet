using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1Tutorial2 : Tutorial
{
    private WaveManager waveManager;

    public override void TutorialEnter()
    {
        base.TutorialEnter();
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag).GetComponent<WaveManager>();
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
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex == 3 , cancellationToken : manager.TutorialCtr.Token);
        manager.SetNextTutorial();
    }
}