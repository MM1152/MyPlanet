using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class TempoModule : Consumable
{
    protected override void ResetItem()
    {
        var towers = towerManager.GetAllTower();
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] != null)
            {
                towers[i].MinusBonusAttackSpeedTopercent(consumData.effect_value);
            }
        }
        GameObject.Destroy(uiTab);
    }
    protected override async UniTaskVoid UseItemAsync(float duration , CancellationTokenSource ctr)
    {
        try
        {
            var towers = towerManager.GetAllTower();
            for (int i = 0; i < towers.Count; i++)
            {
                if (towers[i] != null)
                {
                    towers[i].AddBonusAttackSpeedTopercent(consumData.effect_value);
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