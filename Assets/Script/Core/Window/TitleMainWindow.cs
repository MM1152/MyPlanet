using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleMainWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button selectStageButton;
    [SerializeField] private Button logoutButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI userNickNameText;
    [SerializeField] private TextMeshProUGUI userGold;

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleMainWindow;

        selectStageButton.onClick.AddListener(() => manager.Open(WindowIds.TitleStageSelectedWindow));
        userNickNameText.text = FirebaseManager.Instance.UserData.nickName;
        userGold.text = FirebaseManager.Instance.UserData.gold.ToString();
        logoutButton.onClick.AddListener(() =>
        {
            FirebaseManager.Instance.Logout();
        });
    }

    public override void Open()
    {
        base.Open();
    }
}
