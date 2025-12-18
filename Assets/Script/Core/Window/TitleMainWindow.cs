using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMainWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button selectStageButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button debugModeSceneButton;
    [SerializeField] private Button bookOpenButton;
    [SerializeField] private Button randomPickUpButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI userNickNameText;
    [SerializeField] private TextMeshProUGUI userGold;
    [SerializeField] private TextMeshProUGUI userDiamond;

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleMainWindow;

        FirebaseManager.Instance.Database.AddListner(DataBasePaths.GoldPath, OnChangeGoldValue);
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.DiamondPath, OnChangeDiamondValue);

        selectStageButton.onClick.AddListener(() => manager.Open(WindowIds.TitleStageSelectedWindow));
        userNickNameText.text = FirebaseManager.Instance.UserData.nickName;
        userGold.text = FirebaseManager.Instance.UserData.gold.ToString();
        userDiamond.text = FirebaseManager.Instance.UserData.diamond.ToString();
        logoutButton.onClick.AddListener(() =>
        {
            FirebaseManager.Instance.Logout();
        });
        randomPickUpButton.onClick.AddListener(() => manager.Open(WindowIds.RandomPickUpWindow));
#if UNITY_EDITOR
        debugModeSceneButton.onClick.AddListener(() =>
        {
            LoadingScene.sceneId = SceneIds.DebugModeScene;
            SceneManager.LoadScene(SceneIds.LoadingScene);
        });
        debugModeSceneButton.gameObject.SetActive(true);
#endif
        bookOpenButton.interactable = true;
        bookOpenButton.onClick.AddListener(() =>
        {
            manager.Open(WindowIds.TitleBookWindow);
        });
    }

    public override void Open()
    {
        base.Open();
    }

    private void OnChangeGoldValue(object sender , ValueChangedEventArgs args)
    {
        userGold.text = args.Snapshot.Value.ToString();
    }

    private void OnChangeDiamondValue(object sender, ValueChangedEventArgs args)
    {
        userDiamond.text = args.Snapshot.Value.ToString();
    }
}
