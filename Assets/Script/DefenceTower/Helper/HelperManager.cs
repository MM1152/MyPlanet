using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Helper helper;
    public TowerManager TowerManager => towerManager;

    private List<UserData> userDatas = new List<UserData>();
    private CancellationTokenSource ctr;
    private List<Helper> helpers = new List<Helper>();
    [Header("Settings")]
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int spawnHelperCount = 2;
    private float curInterval;
    private async UniTaskVoid Start()
    {
        if (Variable.IsTutorialActive) return;
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex == 1 , cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
        await GetRandomUserData();
        if(userDatas != null && userDatas.Count > 2)
        {
            for(int i = 0; i < spawnHelperCount; i++) 
            {
                var rand = UnityEngine.Random.Range(0, userDatas.Count);
                var helper = Instantiate(this.helper);
                helper.Init(userDatas[rand], this);
                helpers.Add(helper);
            }
            SpawnHelpers().Forget();
        }
    }

    private async UniTask GetRandomUserData()
    {
        ctr = new CancellationTokenSource();
        ctr.CancelAfterSlim(TimeSpan.FromSeconds(10));

        var userDatas = await FirebaseManager.Instance.Database.GetDatas<UserData>(DataBasePaths.UserPath, ctr);
        if (userDatas.success)
        {
            this.userDatas = userDatas.data;
        }
    }

    private async UniTaskVoid SpawnHelpers()
    {
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex == 3 , cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
        
        while(true)
        {
            await UniTask.Yield(cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
            curInterval += Time.deltaTime;

            if(curInterval >= spawnInterval)
            {
                curInterval = 0f;
                for(int i = 0; i < helpers.Count; i++)
                {
                    var ymax = Screen.height / 2;
                    var xmax = Screen.width + 100f;

                    var randY = UnityEngine.Random.Range(0, ymax);
                    var rand = UnityEngine.Random.Range(0, 1);
                    var randX = rand == 0 ? -100 : xmax;

                    var startPos = Camera.main.ScreenToWorldPoint(new Vector3(randX, randY , -Camera.main.transform.position.z));
                    var endPos = Camera.main.ScreenToWorldPoint(new Vector3(randX == -100 ? xmax : -100 , UnityEngine.Random.Range(0 ,ymax), -Camera.main.transform.position.z));

                    helpers[i].MoveHelper(startPos , endPos);
                }
            }
        }
    }
}
