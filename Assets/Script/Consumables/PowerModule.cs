using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public class PowerModule : Consumable
{
    protected override void ResetItem()
    {
        var towers = towerManager.GetAllTower();
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] != null)
            {
                towers[i].MinusBonusDamageToPercent(consumData.effect_value);
            }
        }
        Release();
        duration = 0f;
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
                    towers[i].AddBonusDamageToPercent(consumData.effect_value);
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