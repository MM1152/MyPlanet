using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class RepairPatch : Consumable
{
    protected override void ResetItem()
    {
        Managers.SoundManager.PlaySFX(AudiosId.magic_light_bubble_01);
        // ���� ��ġ�� ��� ȿ���̹Ƿ� Ư���� ������ ���� ����
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