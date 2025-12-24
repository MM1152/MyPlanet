using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
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
    public static SoundManager SoundManager => Instance.soundManager;
    private TouchManager touchManager;
    private ObjectPoolManager objectPoolManager;
    private SoundManager soundManager;
    private GameObject loadingProgress;
    private bool init;

    private CancellationTokenSource ctr;

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

        var soundManager = new GameObject("SoundManager");
        this.soundManager = soundManager.AddComponent<SoundManager>();
        this.soundManager.transform.SetParent(go.transform);
        await this.soundManager.Init();

        var loadingProgress = await Addressables.LoadAssetAsync<GameObject>("LoadingPanel1").ToUniTask();
        this.loadingProgress = GameObject.Instantiate(loadingProgress, go.transform).transform.GetChild(0).gameObject;

        Application.targetFrameRate = 60;

        init = true;
    }

    public void Release()
    {
        objectPoolManager?.Release();
        soundManager?.StopAllAudioSources();
    }

    public async UniTask WaitForManagerInitalizedAsync()
    {
        await UniTask.WaitUntil(() => init);
    }

    public async UniTask<(T1 , T2)> WaitForLoadingAsync<T1 , T2>(UniTask<(T1 , T2)> task)
    {
        ctr = new CancellationTokenSource();
        ctr.CancelAfterSlim(10000);
        loadingProgress.SetActive(true);
        try
        {
            var data = await UniTask.WhenAll(task).AttachExternalCancellation(ctr.Token);
            loadingProgress.SetActive(false);

            return data[0];
        }
        catch (OperationCanceledException)
        {
            loadingProgress.SetActive(false);
            SceneManager.LoadScene(SceneIds.LoadingScene);
            return default((T1, T2));
        }
    }

    public async UniTask WaitForLoadingAsync(List<UniTask> task)
    {
        ctr = new CancellationTokenSource();
        ctr.CancelAfterSlim(10000);
        loadingProgress.SetActive(true);
        try
        {
            await UniTask.WhenAll(task).AttachExternalCancellation(ctr.Token);
            loadingProgress.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            loadingProgress.SetActive(false);
            SceneManager.LoadScene(SceneIds.LoadingScene);
            return;
        }
    }

    public async UniTask WaitForLoadingAsync(UniTask task)
    {
        ctr = new CancellationTokenSource();
        ctr.CancelAfterSlim(10000);
        loadingProgress.SetActive(true);
        try
        {
            await UniTask.WhenAll(task).AttachExternalCancellation(ctr.Token);
            loadingProgress.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            loadingProgress.SetActive(false);
            SceneManager.LoadScene(SceneIds.LoadingScene);
            return;
        }
    }

    public async UniTask<T> WaitForLoadingAsync<T>(UniTask<T> task)
    {
        ctr = new CancellationTokenSource();
        ctr.CancelAfterSlim(10000);
        loadingProgress.SetActive(true);
        try
        {
            var data = await UniTask.WhenAll(task).AttachExternalCancellation(ctr.Token);
            loadingProgress.SetActive(false);

            return data[0];
        }
        catch (OperationCanceledException)
        {
            loadingProgress.SetActive(false);
            SceneManager.LoadScene(SceneIds.LoadingScene);
            return default(T);
        }

    }
}   