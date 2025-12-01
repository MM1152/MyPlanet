using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class DarkOutputAmplifier : Consumable
{
    protected override void ResetItem()
    {
        if (towerManager != null)
        {
            var darkTowers = towerManager.GetTowerToAttribute(ElementType.Dark);
            for (int i = 0; i < darkTowers.Count; i++)
            {
                if (darkTowers[i] != null)
                {
                    darkTowers[i].MinusBonusDamageToPercent(consumData.effect_value);
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
                var darkTowers = towerManager.GetTowerToAttribute(ElementType.Dark);
                for (int i = 0; i < darkTowers.Count; i++)
                {
                    if (darkTowers[i] != null)
                    {
                        darkTowers[i].AddBonusDamageToPercent(consumData.effect_value);
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