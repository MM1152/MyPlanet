using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserProfilePopup : Popup
{
    [SerializeField] private Image userImage;
    [SerializeField] private TextMeshProUGUI userNickName;
    [SerializeField] private TextMeshProUGUI userEmail;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button closeButton;

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.UserProfilePopup;

        Sprite userIcon = FirebaseManager.Instance.Auth.UserIconSprite;
        if(userIcon != null)
        {
            userImage.sprite = userIcon;
        }

        userNickName.text = FirebaseManager.Instance.Auth.UserDisplayName;
        userEmail.text = string.IsNullOrEmpty(FirebaseManager.Instance.Auth.CurrentUser.Email) ? "게스트 계정으로 로그인 중입니다." : FirebaseManager.Instance.Auth.CurrentUser.Email;

        logoutButton.onClick.AddListener(() =>
        {
            FirebaseManager.Instance.Logout();
        });

        closeButton.onClick.AddListener(() =>
        {
            manager.ForceClose();
        }); 
    }

    public override void Open()
    {
        base.Open();
    }
}
