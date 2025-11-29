using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SteelOutputAmplifier : Consumable
{
    protected override void ResetItem()
    {
        if (towerManager != null)
        {
            var steelTowers = towerManager.GetTowerToAttribute(ElementType.Steel);
            for (int i = 0; i < steelTowers.Count; i++)
            {
                if (steelTowers[i] != null)
                {
                    steelTowers[i].MinusBonusDamageToPercent(consumData.effect_value);
                }
            }
        }
        GameObject.Destroy(uiTab);
    }

    protected override async UniTaskVoid UseItemAsync(float duration, CancellationTokenSource ctr)
    {
        try
        {
            if (towerManager != null)
            {
                var steelTowers = towerManager.GetTowerToAttribute(ElementType.Steel);
                for (int i = 0; i < steelTowers.Count; i++)
                {
                    if (steelTowers[i] != null)
                    {
                        steelTowers[i].AddBonusDamageToPercent(consumData.effect_value);
                    }
                }
            }

            await UniTask.Delay((int)(duration * 1000), cancellationToken: ctr.Token);
        }
        finally
        {
            ResetItem();
        }
    }
}