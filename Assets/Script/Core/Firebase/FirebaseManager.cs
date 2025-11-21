using Cysharp.Threading.Tasks;
using System;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class FirebaseManager
{
    private FirebaseInitalizer initalizer = new FirebaseInitalizer();
    private DataBase database = new DataBase();
    private Auth auth = new Auth();
    private PresetData presetData = new PresetData();
    private UserData userData;

    public string UserId => auth.UserId;
    public UserData UserData => userData;
    public PresetData PresetData => presetData;

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
        Debug.Log("Firebase Auth Initalized");

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

    public async UniTask FindUserDataInDatabase()
    {
        var userPath = DataBasePaths.UserPath + UserId;
        var result = await database.GetData<UserData>(userPath);

        if (result.success)
        {
            // 현재 유저데이터가 존재한다면
            var data =  await database.GetData<UserData>(userPath);
            if(data.success)
            {
                userData = data.data;
                Debug.Log($"Success Load UserData NickName : {userData.nickName}");
            }
        }
        else
        {
            // 현재 유저데이터가 존재하지 않는다면
            UserData newUserData = new UserData();
            var success = await database.OverwriteJsonData(userPath , newUserData);

            if(success)
            {
                userData = newUserData; 
                Debug.Log("Save New UserData");
            }
            else 
            {
                Debug.Log("Save Fail");
            }
        }
    }
}

[Serializable]
public class UserData : JsonSerialized
{
    public string nickName;

    public UserData()
    {
        nickName = "NoName-" + UnityEngine.Random.Range(10000, 50000);
    }
}

