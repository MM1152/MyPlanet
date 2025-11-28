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
                towers[i].MinusBonusDamageToPercent(consumData.duration);
            }
        }
    }

    protected override async UniTaskVoid UseItemAsync(float duration , CancellationTokenSource ctr)
    {
        var towers = towerManager.GetAllTower();
        for(int i = 0; i < towers.Count; i++)
        {
            if (towers[i] != null)
            {
                towers[i].AddBonusDamageToPercent(consumData.duration);
            }
        }

        await UniTask.Delay((int)(duration * 1000), cancellationToken: ctr.Token);
        ResetItem();
    }
}