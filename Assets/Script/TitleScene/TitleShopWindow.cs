using Cysharp.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class TitleShopWindow : Window
{
    [SerializeField] private ShopItemLayout towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI goldText;

    private List<ShopItemLayout> itemLayouts = new List<ShopItemLayout>();
    public override void Close()
    {
        base.Close();
        FirebaseManager.Instance.Database.RemoveListner(DataBasePaths.GoldPath, OnChangeGoldValue);
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleShopWindow;

        var items = DataTableManager.ShopTable.GetAllData();
        var gold = FirebaseManager.Instance.UserData.gold;

        foreach (var item in items)
        {
            var itemList = Instantiate(towerInfomation, towerInfomationRoot);
            itemList.Init(item);
            itemList.OnClick += OnClickItemList;

            if (item.Price > gold)
                itemList.Disable();
            else
                itemList.Enable();

            itemLayouts.Add(itemList);
        }

        goldText.text = gold.ToString("N0");
        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));
    }

    public override void Open()
    {
        FirebaseManager.Instance.Database.RemoveListner(DataBasePaths.GoldPath, OnChangeGoldValue);
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
        textPopup.SetTexts("아이템 구매" , formatingText  , DataTableManager.StringTable.Get(6101) ,  DataTableManager.StringTable.Get(6100));
        textPopup.SetButtonAction(() => OnClickBlueButton(shopTable).Forget(), OnClickRedButton);
    }

    private async UniTaskVoid OnClickBlueButton(ShopTable.Data purchaseData)
    {
        var checkGoldTask = FirebaseManager.Instance.UserData.CheckGoodsAsync(DataBasePaths.GoldPath , purchaseData.Price);
        var result = await Managers.Instance.WaitForLoadingAsync(checkGoldTask);

        if (!result) return;

        var task = FirebaseManager.Instance.UserData.UseGoods(useGoldAmount: purchaseData.Price);
        await Managers.Instance.WaitForLoadingAsync(task);

        var currentTowerData = FirebaseManager.Instance.TowerData.Get(purchaseData.Tower_ID);
        var towerTableData = DataTableManager.TowerTable.Get(purchaseData.Tower_ID);

        popupManager.ForceClose();

        var popup = popupManager.Open<PurchaseTowerPopup>(PopupIds.PurchaseTowerPopup);
        await Managers.Instance.WaitForLoadingAsync(popup.SetPickUpData(towerTableData));

    }

    private void OnChangeGoldValue(object sender , ValueChangedEventArgs args)
    {
        var gold = int.Parse(args.Snapshot.Value.ToString());
        goldText.text = $"{gold:N0}";

        foreach(var layout in itemLayouts)
        {
            if(layout.GetPrice() > gold)
                layout.Disable();
            else
                layout.Enable();
        }
    }

    private void OnClickRedButton()
    {
        popupManager.ForceClose();
    }
}
