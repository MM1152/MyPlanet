using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

public class FirebaseInitalizer : Singleton<FirebaseInitalizer>
{
    private bool initialize = false;

    static FirebaseInitalizer()
    {
        Instance.InitAsync().Forget();
    }

    private async UniTaskVoid InitAsync()
    {
        try
        {
            var task = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if (Firebase.DependencyStatus.Available == task)
            {
#if DEBUG_MODE
                Debug.Log($"Firebase Initalized Success");
#endif
                initialize = true;
            }
            else 
            {
#if DEBUG_MODE
                Debug.LogError($"Firebase Initalized Fail");
#endif
            }
        }
        catch (System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"Firebase Initalized Fail : {ex}");
#endif
        }
    }

    /// <summary>
    /// Wait for Firebase Initalized
    /// </summary>
    /// <returns></returns>
    public async UniTask WaitForInitalizedAsync()
    {
        await UniTask.WaitUntil(() => initialize);
    }
}
