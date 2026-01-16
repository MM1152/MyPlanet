using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1Tutorial3 : Tutorial
{
    private WaveManager waveManager;

    private bool isFirstUpdate = false;
    public override void TutorialEnter()
    {
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag).GetComponent<WaveManager>();
        isFirstUpdate = false;

        base.TutorialEnter();

        WaitForClearAllEnemy().Forget();
    }

    private async UniTask WaitForClearAllEnemy()
    {
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex == 5 , timing : PlayerLoopTiming.PreUpdate);
        manager.SetNextTutorial();
    }
}