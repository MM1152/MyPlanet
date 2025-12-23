using Cysharp.Threading.Tasks;
using UnityEngine;

public class PurchaseTowerPopup : Popup
{
    [SerializeField] private TowerItemViewer towerPikcupViewer;

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.PurchaseTowerPopup;
    }

    public override void Open()
    {
        base.Open();
    }

    public async UniTask SetPickUpData(TowerTable.Data data)
    {
        await UniTask.DelayFrame(2);
        manager.ForceClose();
        var userTowerData = FirebaseManager.Instance.TowerData.Get(data.id);
        var randomOptionTable = DataTableManager.TowerRandomOptionValueTable;
        var randomOptionData = randomOptionTable.GetOptionData(data.id);
        var randomOptionValue = randomOptionTable.GetRandomOptionValue(userTowerData.TowerId, userTowerData.grade , 1);

        var randomItemData = DataTableManager.RandomPickUpTable.GetTowerIdToData(data.id);

        bool isNew = false;
        (bool isDuplication , float value) duplication = (false , 0f);

        if (userTowerData.Unlock)
        {
            // 더 높은값 뽑아서 교체
            if (userTowerData.OptionValue < randomOptionValue.percent)
            {
                var prevOptionValue = userTowerData.OptionValue;
                isNew = false;
                duplication = (true, randomOptionValue.percent);
                await FirebaseManager.Instance.TowerData.UpdateOptionValueAsync(userTowerData, randomOptionValue.percent);
            }
            // 더 낮은값 뽑아서 조각으로 교체
            else
            {
                var duplicationPiece = DataTableManager.TowerDuplicationRewardTable.GetDuplicationPartCount(userTowerData.TowerId, randomOptionData.GetGradeToId(userTowerData.grade), randomOptionValue.LMH);
                isNew = false;
                duplication = (false, duplicationPiece);
                await FirebaseManager.Instance.TowerData.AddPartCountAsync(userTowerData, duplicationPiece);
            }
        }
        else
        {
            // 처음 얻은 타워
            isNew = true;
            duplication = (false, 0f);
            await FirebaseManager.Instance.TowerData.UnlockAsync(userTowerData, randomOptionValue.percent);
        }

        towerPikcupViewer.SetData(randomItemData, isNew, duplication);

        manager.Open<PurchaseTowerPopup>(PopupIds.PurchaseTowerPopup);
    }
}
