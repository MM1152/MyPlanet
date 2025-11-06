using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

public class Auth
{
    private FirebaseAuth auth;
    public string UserId => auth.CurrentUser?.UserId ?? string.Empty;

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

    public void Logout()
    {
        auth.SignOut();
    }
}

