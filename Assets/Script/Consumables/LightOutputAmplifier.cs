using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class LightOutputAmplifier : Consumable
{
    protected override void ResetItem()
    {
        if (towerManager != null)
        {
            var lightTowers = towerManager.GetTowerToAttribute(ElementType.Light);
            for (int i = 0; i < lightTowers.Count; i++)
            {
                if (lightTowers[i] != null)
                {
                    lightTowers[i].MinusBonusDamageToPercent(consumData.effect_value);
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
                var lightTowers = towerManager.GetTowerToAttribute(ElementType.Light);
                for (int i = 0; i < lightTowers.Count; i++)
                {
                    if (lightTowers[i] != null)
                    {
                        lightTowers[i].AddBonusDamageToPercent(consumData.effect_value);
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