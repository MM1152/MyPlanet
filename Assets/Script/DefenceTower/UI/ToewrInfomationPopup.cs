using UnityEngine;

public class ToewrInfomationPopup : Popup
{
    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.TowerInfomationPopup;
    }

    public override void Open()
    {
        base.Open();
    }
}
