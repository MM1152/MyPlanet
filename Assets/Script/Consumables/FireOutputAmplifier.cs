using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class FireOutputAmplifier : Consumable
{
    protected override void ResetItem()
    {
        if (towerManager != null)
        {
            var fireTowers = towerManager.GetTowerToAttribute(ElementType.Fire);
            for (int i = 0; i < fireTowers.Count; i++)
            {
                if (fireTowers[i] != null)
                {
                    fireTowers[i].MinusBonusDamageToPercent(consumData.effect_value);
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
                var fireTowers = towerManager.GetTowerToAttribute(ElementType.Fire);
                for (int i = 0; i < fireTowers.Count; i++)
                {
                    if (fireTowers[i] != null)
                    {
                        fireTowers[i].AddBonusDamageToPercent(consumData.effect_value);
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