using UnityEngine;
using UnityEngine.UI;

public class SideBarPopup : Popup
{
    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button eventButton;
    [SerializeField] private Button attendanceButton;
    [SerializeField] private Button bookButton;
    [SerializeField] private Button randomPickButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;

    [Header("Reference")]
    [SerializeField] private WindowManager windowManager;
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
        windowManager.Open(WindowIds.TitleStackRewardPlayWindow);
        OnClickCloseButton();
    }

    private void OnClickAttendanceButton()
    {
        //FIX
        windowManager.Open(WindowIds.TitleDailyGiftWindow);
        OnClickCloseButton();
    }

    private void OnClickBookButton()
    {
        //FIX
        windowManager.Open(WindowIds.TitleBookWindow);
        OnClickCloseButton();
    }

    private void OnClickRandomPickUpButton()
    {
        //FIX
        windowManager.Open(WindowIds.RandomPickUpWindow);
        OnClickCloseButton();
    }

    private void OnClickShopButton()
    {
        windowManager.Open(WindowIds.TitleShopWindow);
        OnClickCloseButton();
    }

    private void OnClickOptionButton()
    {
        //FIX
    }

    private void OnClickExitButton()
    {
        manager.ForceClose();
        var popup = manager.Open<TextPopup>(PopupIds.TextPopup);
        popup.SetTexts("게임 종료", "정말로 게임을 종료하시겠습니까?", "게임종료", "돌아가기");
        popup.SetButtonAction(() => popup.Close(),() => Application.Quit());
        popup.SetButtonAudio(AudiosId.ui_button_simple_click_05, AudiosId.ui_button_simple_click_05);
    }
}
