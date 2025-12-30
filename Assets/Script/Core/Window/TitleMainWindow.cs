using Firebase.Database;
using System.Runtime.CompilerServices;
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
    [SerializeField] private TextMeshProUGUI userExpText;
    [Header("Images")]
    [SerializeField] private Image userProfileIconImage;
    [Header("References")]
    [SerializeField] private TutorialManager tutorialManager;

    public Button SelectStageButton => selectStageButton;
    public Button GachaButton => randomPickUpButton;

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
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.ExpPath, OnChangeExpValue);

        selectStageButton.onClick.AddListener(() => manager.Open(WindowIds.TitleStageSelectedWindow));

        userNickNameText.text = FirebaseManager.Instance.Auth.UserDisplayName;
        userProfileIconImage.sprite = FirebaseManager.Instance.Auth.UserIconSprite;

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
        bookOpenButton.onClick.AddListener(() =>
        {
            manager.Open(WindowIds.TitleBookWindow);
        });
    }

    public override void Open()
    {
        base.Open();
        if (FirebaseManager.Instance.UserData.isClearStage1Tutorial)
        {
            if (!FirebaseManager.Instance.UserData.isClearRandomPickUpTutorial)
            {
                tutorialManager.InitTutorial(TutorialStep.PickUp);
            }
        }

        if (!FirebaseManager.Instance.UserData.isClearFirstTutorial)
        {
            tutorialManager.InitTutorial(TutorialStep.Stage1Enter);
        }
         
        Managers.SoundManager.PlayBGM(AudiosId.A_Dope_Chill_Session);   
    }

    private void OnChangeGoldValue(object sender , ValueChangedEventArgs args)
    {
        userGold.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

    private void OnChangeDiamondValue(object sender, ValueChangedEventArgs args)
    {
        userDiamond.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

    private void OnChangeExpValue(object sender, ValueChangedEventArgs args)
    {
        userExpText.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }
}
