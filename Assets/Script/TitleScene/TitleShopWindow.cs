using UnityEngine;

public class TitleShopWindow : Window
{
    [SerializeField] private ShopItemLayout towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private PopupManager popupManager;

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
    }

    public override void Open()
    {
        base.Open();
    }

    private void OnClickItemList(ShopTable.Data shopTable)
    {
        var textPopup = popupManager.Open<TextPopup>(PopupIds.TextPopup);
        textPopup.SetTexts("아이템 구매" , DataTableManager.StringTable.Get(6099) , DataTableManager.StringTable.Get(6100) , DataTableManager.StringTable.Get(6101));
    }
}
