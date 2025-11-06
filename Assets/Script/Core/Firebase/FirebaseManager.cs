using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class FirebaseManager
{
    private FirebaseInitalizer initalizer = new FirebaseInitalizer();
    private DataBase database = new DataBase();
    private Auth auth = new Auth();

    private bool initialize = false;

    private static FirebaseManager instance;
    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseManager();
            }

            return instance;
        }
    }
     
    public FirebaseInitalizer Initalizer => initalizer;
    public DataBase Database => database;
    public Auth Auth => auth;

    private FirebaseManager() {}
    static FirebaseManager()
    {
        Instance.InitAsync().Forget();
    }

    private async UniTask InitAsync()
    {
        await initalizer.InitAsync();
        Debug.Log("Firebase Initalized");
        database.Init();
        Debug.Log("Firebase Database Initalized");
        auth.Init();
        Debug.Log("Firebase Database Initalized");

        initialize = true;
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