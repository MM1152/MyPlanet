using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.Networking;

public class Auth
{
    private FirebaseAuth auth;
    public string UserId => auth.CurrentUser?.UserId ?? string.Empty;
    public FirebaseUser CurrentUser => auth.CurrentUser;
    public string UserDisplayName => string.IsNullOrEmpty(auth.CurrentUser.DisplayName) ? FirebaseManager.Instance.UserData.nickName : auth.CurrentUser.DisplayName;
    public Sprite UserIconSprite { get; set; }

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public async UniTask<(string id, bool sucess)> SignInAnonymouslyAsync()
    {
        try
        {
            AuthResult result = await auth.SignInAnonymouslyAsync().AsUniTask();

            if(result != null)
            {
                return (UserId, true);
            }

            return (string.Empty, false);
        }
        catch(System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"[Auth] SignInAnonymouslyAsync failed: {ex}");
#endif
            return (string.Empty, false);
        }
    }

    public async UniTask<(string id, bool sucess)> SignInGoogleLoginAsync()
    {
        try
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                WebClientId = "596297009977-hr66tt0ebbvj4r1g4qrrfhpasb5adgd1.apps.googleusercontent.com",
                RequestIdToken = true,
                UseGameSignIn = false,
                RequestEmail = true,
                RequestProfile = true,
            };

            // 로그인 이후 토큰값 불러옴
            GoogleSignInUser userData = await GoogleSignIn.DefaultInstance.SignIn().AsUniTask();

            // 해당 토큰값으로 파이어베이스 로그인 시도
            Credential credential = GoogleAuthProvider.GetCredential(userData.IdToken, null);

            await auth.SignInWithCredentialAsync(credential).AsUniTask();
            return (auth.CurrentUser.DisplayName, true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Auth] SignInGoogleLoginAsync failed: {ex}");

            return (null, false);
        }
    }

    public async UniTask<Sprite> DownLoadIconImage(System.Uri path)
    {
        var www = UnityWebRequestTexture.GetTexture(path);

        var result = await www.SendWebRequest().ToUniTask();
        if(result.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(result.error);
            return null;
        }

        Texture2D iconImage = ((DownloadHandlerTexture)result.downloadHandler).texture;
        return Sprite.Create(iconImage , new Rect(0,0,iconImage.width,iconImage.height), Vector2.one * 0.5f);
    }

    public void Logout()
    {
        auth.SignOut();
    }
}

