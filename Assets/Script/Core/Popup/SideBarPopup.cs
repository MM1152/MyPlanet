using UnityEngine;
using UnityEngine.UI;

public class SideBarPopup : Popup
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button eventButton;
    [SerializeField] private Button attendanceButton;
    [SerializeField] private Button bookButton;
    [SerializeField] private Button randomPickButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.SideBarPopup;
    }

    public override void Open()
    {
        base.Open();
    }
}
