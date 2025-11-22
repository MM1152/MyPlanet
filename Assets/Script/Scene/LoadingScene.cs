using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI currentProgress;
    public static string sceneId = "TitleScene";

    [Header("Firebase AuthLogin")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button annymousLoginButton;

    public async UniTaskVoid Start()
    {
        loginPanel.SetActive(false);
        annymousLoginButton.onClick.AddListener(() =>
        {
            WaitForLoginAsync().Forget();
            annymousLoginButton.interactable = false;
        });
        await LoadSceneAsync(sceneId);
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
        }
    }

    public async UniTask LoadSceneAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        Managers.Instance.Release();

        currentProgress.text = "Firebase 초기화 중";
        await FirebaseManager.Instance.WaitForInitalizedAsync();

        //Firebase Auth 로그인 정보가 존재할때 다음 작업 진행가능하도록 기다림
        if(FirebaseManager.Instance.UserId == string.Empty)
        {
            loginPanel.SetActive(true);
            currentProgress.text = "로그인 대기 중";
        }
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserId != string.Empty);

        currentProgress.text = "유저 데이터 불러오는 중";
        if (FirebaseManager.Instance.UserData == null)
        {
            await FirebaseManager.Instance.FindUserDataInDatabase();    
        }

        if (!FirebaseManager.Instance.PresetData.Init)
        {
            await FirebaseManager.Instance.PresetData.Load();
        }

        currentProgress.text = "Managers 초기화 중";
        await Managers.Instance.WaitForManagerInitalizedAsync();
        currentProgress.text = "테이블 불러오는 중";
        await DataTableManager.WaitForInitalizeAsync();
        currentProgress.text = "Scene 초기화 중";
        await Addressables.LoadSceneAsync(sceneId).ToUniTask();
    }
}
