using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class Managers
{
    private static Managers instance;
    public static Managers Instance
    {
        get
        {
            if( instance == null)
            {
                instance = new Managers();
                instance.Init().Forget();
            }

            return instance;
        }
    }

    public static TouchManager TouchManager => Instance.touchManager;
    public static ObjectPoolManager ObjectPoolManager => Instance.objectPoolManager;

    private TouchManager touchManager;
    private ObjectPoolManager objectPoolManager;
    private GameObject loadingProgress;
    private bool init;

    private async UniTaskVoid Init()
    {
        var go = new GameObject("Managers");
        GameObject.DontDestroyOnLoad(go);

        var touchManager = new GameObject("TouchManager");
        this.touchManager = touchManager.AddComponent<TouchManager>();
        this.touchManager.Init();
        this.touchManager.transform.SetParent(go.transform);

        var objectPoolManager = new GameObject("ObjectPoolManager");
        this.objectPoolManager = objectPoolManager.AddComponent<ObjectPoolManager>();
        this.objectPoolManager.transform.SetParent(go.transform);
        await this.objectPoolManager.Init();

        var loadingProgress = await Addressables.LoadAssetAsync<GameObject>("LoadingPanel1").ToUniTask();
        this.loadingProgress = GameObject.Instantiate(loadingProgress, go.transform);

        init = true;
    }

    public void Release()
    {
        objectPoolManager?.Release();
    }

    public async UniTask WaitForManagerInitalizedAsync()
    {
        await UniTask.WaitUntil(() => init);
    }

    public async UniTask<(T1 , T2)> WaitForLoadingAsync<T1 , T2>(UniTask<(T1 , T2)> task)
    {
        loadingProgress.SetActive(true);
        var data = await UniTask.WhenAll(task);
        loadingProgress.SetActive(false);

        return data[0];
    }
}   