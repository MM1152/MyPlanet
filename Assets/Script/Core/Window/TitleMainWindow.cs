using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleMainWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button selectStageButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI userNickNameText;
    
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
    }

    public override void Open()
    {
        base.Open();
    }
}
