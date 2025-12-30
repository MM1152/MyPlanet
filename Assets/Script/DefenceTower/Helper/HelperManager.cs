using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Helper helper;
    public TowerManager TowerManager => towerManager;

    private List<AsyncPlayerData.Data> userDatas = new List<AsyncPlayerData.Data>();
    private CancellationTokenSource ctr;
    private List<Helper> helpers = new List<Helper>();
    [Header("Settings")]
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int spawnHelperCount = 2;
    private float curInterval;

    [SerializeField] private HelperViewer[] helperViewers;
    public HelperViewer[] HelperViewers => helperViewers;
    private async UniTaskVoid Start()
    {
        helperViewers[0].gameObject.SetActive(false);
        helperViewers[1].gameObject.SetActive(false);

        if (Variable.IsTutorialActive) return;

        SaveUserData().Forget();
        await GetRandomUserData();

        if (userDatas != null && userDatas.Count >= 2)
        {
            for (int i = 0; i < spawnHelperCount; i++)
            {
                var rand = UnityEngine.Random.Range(0, userDatas.Count);
                var helper = Instantiate(this.helper);
                helper.Init(userDatas[i], this , helperViewers[i]);
                helpers.Add(helper);
            }
            SpawnHelpers().Forget();
        }

    }

    private async UniTask SaveUserData()
    {
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex / (float)waveManager.MaxWave >= 0.4f);

        var stageId = FirebaseManager.Instance.PresetData.GetGameData().stageId;
        var asyncUserData = new AsyncPlayerData.Data()
        {
            playerNickName = FirebaseManager.Instance.UserData.nickName,
            playerPlanetId = FirebaseManager.Instance.PresetData.GetGameData().data.PlanetId,
            playerTowerIds = towerManager.GetAllTower().Select(tower => tower != null ? tower.TowerData.ID : -1).ToList(),
            //playerTowerLevels = towerManager.GetAllTower().Select(tower => tower.Level).ToList(),
            playerTowerFullDamages = towerManager.GetAllTower().Select(tower => tower != null ? tower.FullDamage : -1).ToList(),
            imageUrl = FirebaseManager.Instance.Auth.CurrentUser.PhotoUrl.ToString(),
        };

        await FirebaseManager.Instance.AsyncPlayerData.SaveAsyncData(stageId, asyncUserData);
    }

    private async UniTask GetRandomUserData()
    {
        ctr = new CancellationTokenSource();
        ctr.CancelAfterSlim(TimeSpan.FromSeconds(10));

        var stageId = FirebaseManager.Instance.PresetData.GetGameData().stageId;

        // ¼ÅÇÃÇØ¼­ µ¹·ÁÁÜ
        var asyncUserData = await FirebaseManager.Instance.AsyncPlayerData.LoadAsyncData(stageId);

        if (asyncUserData.isSuccess && asyncUserData.datas.Count >= 2)
        {
            this.userDatas.Add(asyncUserData.datas[0]);
            this.userDatas.Add(asyncUserData.datas[1]);

            helperViewers[0].SetUserData(this.userDatas[0]);
            helperViewers[1].SetUserData(this.userDatas[1]);
        }
    }

    private async UniTaskVoid SpawnHelpers()
    {
        await UniTask.WaitUntil(() => waveManager.CurrentWaveIndex == waveManager.MaxWave , cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());

        helperViewers[0].SetActive(true);
        helperViewers[1].SetActive(true);

        while (true)
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
