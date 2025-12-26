using Cysharp.Threading.Tasks;
using Firebase.Database;
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
    private TowerData towerData = new TowerData();
    private AsyncPlayerData asyncPlayerData = new AsyncPlayerData();
    private UserData userData;

    public string UserId => auth.UserId;
    public UserData UserData => userData;
    public PresetData PresetData => presetData;
    public PlanetData PlanetData => planetData;
    public TowerData TowerData => towerData;
    public AsyncPlayerData AsyncPlayerData => asyncPlayerData;

    private bool initialize = false;

    private long serverTime;
    public long ServerTime => serverTime;

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
            towerData.LoadAsync().Forget();

            initialize = true;
        }

        serverTime = await database.GetServerTime();
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

            await UpdateDataToNewVersion(data.data , userPath);


            if (data.success)
            {
                userData = data.data;
                Debug.Log($"Success Load UserData NickName : {userData.nickName}");
            }

            var playerPlayTime = Utils.CovertLongToServerTime(data.data.playTime);
            var curServerTime = Utils.CovertLongToServerTime(serverTime);

            if(playerPlayTime.Date < curServerTime.Date)
            {
                await data.data.ChangeDateToUpdatData();
                data.data.playTime = serverTime;
                await userData.SaveAsync(userPath , data.data);
            }

        }
        else
        {
            // 현재 유저데이터가 존재하지 않는다면
            UserData newUserData = new UserData();
            await newUserData.InitalizedUserData();

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

    private async UniTask UpdateDataToNewVersion(UserData data , string userPath)
    {
        if (data.version != Version)
        {
            changeVersion = true;
            data.version = Version;
            bool success = await Database.OverwriteJsonData<UserData>(userPath, data); 
            if (success)
            {
                Debug.Log("Update UserData Success");
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
        towerData.Release();
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
    public int diamond;
    public bool isClearPresetTutorial;
    public bool isClearRandomPickUpTutorial;
    public bool isClearBookTutorial;
    public bool isClearStage1Tutorial;
    public bool isClearFirstTutorial;
    public bool isClearStage2Tutorial;

    public int[] getDailyGift;
    public int dailyGiftDate;

    public int clearWaveCount;
    public int[] stackRewards;

    public int version;
    public long playTime;

    public async UniTask InitalizedUserData()
    {
        nickName = "NoName-" + UnityEngine.Random.Range(10000, 50000);
        gold = 0;
        exp = 0;

        isClearPresetTutorial = false;
        isClearStage2Tutorial = false;

        getDailyGift = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        dailyGiftDate = 0;

        stackRewards = new int[] { 0, 0, 0 };
        clearWaveCount = 0;

        version = FirebaseManager.Instance.Version;
        playTime = await FirebaseManager.Instance.Database.GetServerTime();
    }

    public async UniTask ChangeDateToUpdatData()
    {
        dailyGiftDate = Math.Clamp(++dailyGiftDate,0, 13);
        stackRewards = new int[] { 0, 0, 0 };
        clearWaveCount = 0;
    }

    public async UniTask SaveDailyGift(int day)
    {
        getDailyGift[day] = 1;
        await SaveAsync(DataBasePaths.UserPath + FirebaseManager.Instance.UserId , this);
    }

    public async UniTask GetGoods(int gold = 0, int exp = 0, int diamond = 0)
    {
        this.gold += gold;
        this.exp += exp;
        this.diamond += diamond;
        await SaveAsync(DataBasePaths.UserPath + FirebaseManager.Instance.UserId , this);
    }

    public async UniTask UseGoods(int useGoldAmount = 0, int useExpAmount = 0 , int useDiaAmount = 0)
    {
        this.gold -= useGoldAmount;
        this.exp -= useExpAmount;
        this.diamond -= useDiaAmount;
        await SaveAsync(DataBasePaths.UserPath + FirebaseManager.Instance.UserId , this);
    }

    public async UniTask ClearPresetTutorial()
    {
        isClearPresetTutorial = true;
        await SaveAsync(DataBasePaths.UserPath + FirebaseManager.Instance.UserId, this);
    }

    public async UniTask SaveClearWaveCount(int clearWave)
    {
        clearWaveCount += clearWave;
        await SaveAsync(DataBasePaths.UserPath + FirebaseManager.Instance.UserId, this);
    }

    public async UniTask SaveAsync(string path , UserData userData)
    {
        var success = await FirebaseManager.Instance.Database.OverwriteJsonData<UserData>(path , userData);
    }

    public async UniTask<bool> CheckGoodsAsync(string path , int goods)
    {
        var data = await FirebaseManager.Instance.Database.GetDataToValue(path);

        if(data.success)
        {
            if(goods <= int.Parse(data.data.ToString()))
            {
                return true;
            }
        }
        return false;
    }
}

