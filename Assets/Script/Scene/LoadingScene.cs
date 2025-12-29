using Cysharp.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI currentProgress;
    public TextMeshProUGUI loadingText; 
    public static string sceneId = "TitleScene";
    public float textAnimationDelay = 0.2f;
    [Header("Firebase AuthLogin")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button annymousLoginButton;
    [SerializeField] private Button googleLoginButton;

    private CancellationTokenSource ctr;

    public async UniTaskVoid Start()
    {
        loginPanel.SetActive(false);
        annymousLoginButton.onClick.AddListener(() =>
        {
            WaitForLoginAsync().Forget();
            annymousLoginButton.interactable = false;
            googleLoginButton.interactable = false;
        });

        googleLoginButton.onClick.AddListener(() =>
        {
            WaitForGoogleLoginAsync().Forget();
            annymousLoginButton.interactable = false;
            googleLoginButton.interactable = false;
        });


        await LoadSceneAsync(sceneId);
    } 

    private async UniTaskVoid WaitForGoogleLoginAsync()
    {
        (string userId , bool success) = await FirebaseManager.Instance.Auth.SignInGoogleLoginAsync();
        if(success)
        {
            loginPanel.SetActive(false);
        }
        else
        {
            annymousLoginButton.interactable = true;
            googleLoginButton.interactable = true;
        }
    }

    private async UniTaskVoid WaitForLoginAsync()
    {
        (string userId , bool success) = await FirebaseManager.Instance.Auth.SignInAnonymouslyAsync();
        if(success)
        {
            loginPanel.SetActive(false);
        }
        else
        {
            annymousLoginButton.interactable = true;
            googleLoginButton.interactable = true;
        }
    }

    public async UniTask LoadSceneAsync(string id)
    {
        ctr = new CancellationTokenSource();
        if (string.IsNullOrEmpty(id))
        {
            return;
        }
        TextAniamtionAsync(ctr).Forget();
        Time.timeScale = GameSpeed.ResetGameSpeed();
        Variable.IsSpawnActive = true;
        Variable.IsTutorialActive = false;
        Variable.IsJoyStickActive = true;
        Variable.IsDebugMode = false;

        Managers.Instance.Release();

        currentProgress.text = "4팀 레츠고 좀만 더 고생하자.";
        await DataTableManager.WaitForInitalizeAsync();

        await FirebaseManager.Instance.WaitForInitalizedAsync();

        //Firebase Auth 로그인 정보가 존재할때 다음 작업 진행가능하도록 기다림
        if(FirebaseManager.Instance.UserId == string.Empty)
        {
            loginPanel.SetActive(true);
        }
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserId != string.Empty);

        if (FirebaseManager.Instance.UserData == null)
        {
            await FirebaseManager.Instance.FindUserDataInDatabase();    
        }
        FirebaseManager.Instance.Release();
        await FirebaseManager.Instance.PresetData.WaitForInitalizeAsync();
        await FirebaseManager.Instance.PlanetData.WaitForInitalizeAsync();
        await FirebaseManager.Instance.TowerData.WaitForInitializeAsync();

        await Managers.Instance.WaitForManagerInitalizedAsync();

        if(sceneId == SceneIds.GameScene)
        {
            await LoadGameData();
        }

        ctr?.Cancel();
        ctr?.Dispose();

        loadingText.text = "Game Start";

        await UniTask.WaitUntil(() => Managers.TouchManager.TouchType == TouchTypes.Tab);
        await Addressables.LoadSceneAsync(sceneId).ToUniTask();
    }

    private async UniTask LoadGameData()
    {
        var inGameData = FirebaseManager.Instance.PresetData.GetGameData();
        await DataTableManager.PlanetTable.LoadPlanetPrefab(inGameData.data.PlanetId);
    }

    private async UniTaskVoid TextAniamtionAsync(CancellationTokenSource ctr)
    {
        string text = "Now Loading...";
        int textPointer = 0;
        StringBuilder sb = new StringBuilder();

        while (true)
        {
            if(ctr.IsCancellationRequested)
            {
                break;
            }

            if(textPointer >= text.Length)
            {
                sb.Clear();
                textPointer = 0;
            }

            sb.Append(text[textPointer++]);
            loadingText.text = sb.ToString();
            await UniTask.Delay((int)(textAnimationDelay * 1000));
        }
    }
}
