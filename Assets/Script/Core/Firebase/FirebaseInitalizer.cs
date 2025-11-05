using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

public class FirebaseInitalizer
{
    public async UniTask InitAsync()
    {
        try
        {
            var task = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if (Firebase.DependencyStatus.Available == task)
            {
#if DEBUG_MODE
                Debug.Log($"Firebase Initalized Success");
#endif
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


}
