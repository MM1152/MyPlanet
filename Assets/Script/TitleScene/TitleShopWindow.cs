using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TitleShopWindow : Window
{
    [SerializeField] private ShopItemLayout towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private Button backButton;
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleShopWindow;

        var items = DataTableManager.ShopTable.GetAllData();

        foreach(var item in items)
        {
            var itemList = Instantiate(towerInfomation, towerInfomationRoot);
            itemList.Init(item);
            itemList.OnClick += OnClickItemList;
        }

        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));
    }

    public override void Open()
    {
        base.Open();
    }

    private void OnClickItemList(ShopTable.Data shopTable)
    {
        var textPopup = popupManager.Open<TextPopup>(PopupIds.TextPopup);
        var formatingText = Utils.FormatText(
            DataTableManager.StringTable.Get(6099),
            ("Price", shopTable.Price.ToString()),
            ("Name", DataTableManager.TowerTable.Get(shopTable.Tower_ID).Name)
        );
        textPopup.SetTexts("아이템 구매" , formatingText , DataTableManager.StringTable.Get(6100) , DataTableManager.StringTable.Get(6101));
        textPopup.SetButtonAction(OnClickBlueButton , () => OnClickRedButton(shopTable).Forget());
    }

    private void OnClickBlueButton()
    {
        popupManager.ForceClose();
    }

    private async UniTaskVoid OnClickRedButton(ShopTable.Data purchaseData)
    {
        var checkGoldTask = FirebaseManager.Instance.UserData.CheckGoodsAsync(DataBasePaths.GoldPath , purchaseData.Price);
        var result = await Managers.Instance.WaitForLoadingAsync(checkGoldTask);

        if (!result) return;

        var currentTowerData = FirebaseManager.Instance.TowerData.Get(purchaseData.Tower_ID);
        var towerTableData = DataTableManager.TowerTable.Get(purchaseData.Tower_ID);

        if (!currentTowerData.Unlock)
        {
            currentTowerData.Unlock = true;
        }

        //FIX : 임시로 랜덤 옵션 값 부여
        var optionValue = Random.Range(towerTableData.Min_Value, towerTableData.Max_Value);
        
        if (optionValue > currentTowerData.OptionValue)
        {
            currentTowerData.OptionValue = optionValue;
        }

        var task = FirebaseManager.Instance.TowerData.Save(currentTowerData);
        var useGoldTask = FirebaseManager.Instance.UserData.UseGoods(purchaseData.Price , 0);

        var success = await Managers.Instance.WaitForLoadingAsync(task);
        await Managers.Instance.WaitForLoadingAsync(useGoldTask);

#if DEBUG_MODE
        if (success.Item1)
             Debug.Log("타워 데이터 저장성공");
#endif
        popupManager.ForceClose();
    }
}
