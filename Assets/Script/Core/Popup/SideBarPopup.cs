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

        closeButton.onClick.AddListener(OnClickCloseButton);
        eventButton.onClick.AddListener(OnClickEventButton);
        attendanceButton.onClick.AddListener(OnClickAttendanceButton);
        bookButton.onClick.AddListener(OnClickBookButton);
        randomPickButton.onClick.AddListener(OnClickRandomPickUpButton);
        shopButton.onClick.AddListener(OnClickShopButton);
        optionButton.onClick.AddListener(OnClickOptionButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public override void Open()
    {
        base.Open();
    }

    private void OnClickCloseButton()
    {
        manager.ForceClose();
    }

    private void OnClickEventButton()
    {
        //FIX
    }

    private void OnClickAttendanceButton()
    {
        //FIX
    }

    private void OnClickBookButton()
    {
        //FIX
    }

    private void OnClickRandomPickUpButton()
    {
        //FIX
    }

    private void OnClickShopButton()
    {
        //FIX
    }

    private void OnClickOptionButton()
    {
        //FIX
    }

    private void OnClickExitButton()
    {
        Application.Quit();
    }
}
