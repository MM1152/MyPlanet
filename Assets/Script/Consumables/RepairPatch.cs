using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class RepairPatch : Consumable
{
    protected override void ResetItem()
    {
        // 복구 패치는 즉시 효과이므로 특별히 리셋할 것이 없음
        GameObject.Destroy(uiTab);
    }

    protected override async UniTaskVoid UseItemAsync(float duration, CancellationTokenSource ctr)
    {
        try
        {
            planet.RepairHpToPercent(consumData.effect_value);
            await UniTask.Delay(consumData.duration, cancellationToken: ctr.Token);
        }
        finally
        {
            ResetItem();
        }
    }
}