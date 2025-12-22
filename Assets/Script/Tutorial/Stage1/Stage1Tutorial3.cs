using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1Tutorial3 : Tutorial
{
    private WaveManager waveManager;

    private string[] msgs =
    {
        "불-금속-냉기-불 순으로 상성이 돌고\n빛과 어둠은 서로에게 강합니다",
        "유리한 속성 공격은 1.5배,\n불리한 속성 공격은 0.5배의 피해가 적용됩니다"
    };

    private float delay = 0.5f;
    private bool isFirstUpdate = false;
    public override void TutorialEnter()
    {
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag).GetComponent<WaveManager>();
        isFirstUpdate = false;
        SetTextWithAnimation(msgs[0], backGroundRayCastAble : false).Forget();
        WaitForClearAllEnemy().Forget();
    }

    public override void TutorialExit()
    {
        
    }

    public override void TutorialUpdate()
    {
        if(!isFirstUpdate && manager.GetActiveTutorialTextEndImage())
        {
            delay -= Time.deltaTime;
            if(Managers.TouchManager.TouchType == TouchTypes.Tab || delay <= 0f)
            {
                SetTextWithAnimation(msgs[1], backGroundRayCastAble: false).Forget();
                isFirstUpdate = true;
            }
        }
    }

    private async UniTask WaitForClearAllEnemy()
    {
        await UniTask.WaitUntil(() => waveManager.waveClearCount == 0 , timing : PlayerLoopTiming.PreUpdate);
        manager.SetNextTutorial();
    }
}