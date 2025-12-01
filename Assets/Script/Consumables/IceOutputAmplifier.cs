using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class IceOutputAmplifier : Consumable
{
    protected override void ResetItem()
    {
        if (towerManager != null)
        {
            var iceTowers = towerManager.GetTowerToAttribute(ElementType.Ice);
            for (int i = 0; i < iceTowers.Count; i++)
            {
                if (iceTowers[i] != null)
                {
                    iceTowers[i].MinusBonusDamageToPercent(consumData.effect_value);
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
                var iceTowers = towerManager.GetTowerToAttribute(ElementType.Ice);
                for (int i = 0; i < iceTowers.Count; i++)
                {
                    if (iceTowers[i] != null)
                    {
                        iceTowers[i].AddBonusDamageToPercent(consumData.effect_value);
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