using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class DefenseShieldGenerator : Consumable
{
    protected override void ResetItem()
    {
        // 보호막 효과가 끝나면 자동으로 제거됨 (지속시간 종료)
        GameObject.Destroy(uiTab);
    }

    protected override async UniTaskVoid UseItemAsync(float duration, CancellationTokenSource ctr)
    {
        try
        {
            if (planet != null)
            {
                // 즉시 보호막 레이어를 생성하여 일정 비율의 피해를 흡수하도록 하는 방어 장치
                // 최대 체력의 일정 비율 보호막 생성
                planet.AddShieldToPercent(consumData.effect_value);
            }
            
            await UniTask.Delay((int)(duration * 1000), cancellationToken: ctr.Token);
        }
        finally
        {
            ResetItem();
        }
    }
}