using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirebaseManager
{
    private FirebaseInitalizer initalizer = new FirebaseInitalizer();
    private DataBase database = new DataBase();
    private Auth auth = new Auth();
    private PresetData presetData = new PresetData();
    private PlanetData planetData = new PlanetData();
    private UserData userData;

    public string UserId => auth.UserId;
    public UserData UserData => userData;
    public PresetData PresetData => presetData;
    public PlanetData PlanetData => planetData;

    private bool initialize = false;

    private bool changeVersion = false;
    public bool ChangeVersion => changeVersion;

    private int version;
    public int Version => version;

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
        database.Init();
        auth.Init();

        var result = await database.GetVersion();
        if(result.success)
        {
            version = result.version;
            planetData.LoadAllDataAsync().Forget();
            presetData.LoadAsync().Forget();

            initialize = true;
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

    public async UniTask FindUserDataInDatabase()
    {
        var userPath = DataBasePaths.UserPath + UserId;
        var result = await database.GetData<UserData>(userPath);

        if (result.success)
        {
            // 현재 유저데이터가 존재한다면
            var data =  await database.GetData<UserData>(userPath);

            if(data.data.version != Version)
            {
                changeVersion = true;
                data.data.version = Version;
                bool success = await Database.OverwriteJsonData<UserData>(userPath, data.data);
                if(success)
                {
                    Debug.Log("Update UserData Success");
                }
            }

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

    public void Release()
    {
        database.Release();
    }

    public void Logout()
    {
        auth.Logout();
        userData = null;
        presetData.Release();
        planetData.Release();
        InitAsync().Forget();

        LoadingScene.sceneId = SceneIds.TitleScene;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }
}

[Serializable]
public class UserData : JsonSerialized
{
    public string nickName;
    public int gold;
    public int exp;
    public int version;
    public UserData()
    {
        nickName = "NoName-" + UnityEngine.Random.Range(10000, 50000);
        gold = 0;
        exp = 0;
        version = FirebaseManager.Instance.Version;
    }

    public async UniTask UseGoods(int useGoldAmount , int useExpAmount)
    {
        this.gold -= useGoldAmount;
        this.exp -= useExpAmount;

        await SaveGoodsAsync(DataBasePaths.UserPath + FirebaseManager.Instance.UserId , this);
    }

    public async UniTask SaveGoodsAsync(string path , UserData userData)
    {
        var success = await FirebaseManager.Instance.Database.OverwriteJsonData<UserData>(path , userData);
    }
}

