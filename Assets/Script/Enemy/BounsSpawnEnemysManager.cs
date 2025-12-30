using System.Collections.Generic;
using UnityEngine;

public class BounsSpawnEnemysManager : MonoBehaviour
{
    private Enemy body;
    private List<BounsSpawnEnemysTable.Data> spawnGettingsList = new List<BounsSpawnEnemysTable.Data>();
    private List<BounsSpawnEnemysTable.Data> spawnSettingsList = new List<BounsSpawnEnemysTable.Data>();
    private List<float> spawnIntervals = new List<float>();
    private bool initialized = false;
    private EnemySpawnManager enemySpawnManager;
    private WaveManager waveManager;
    public void Initialize(int bossId, Enemy enemy)
    {
        if (!SetSpawnSettings(bossId)) return;
        this.enabled = true;
        this.body = enemy;
        spawnSettingsList = new List<BounsSpawnEnemysTable.Data>(spawnGettingsList);
        spawnIntervals.Clear();
        foreach (var d in spawnGettingsList)
        {
            spawnIntervals.Add(d.INTERVAL);
        }
        initialized = true;
        enemySpawnManager = enemy.EnemySpawnManager;
        waveManager = enemy.WaveManager;
    }

    private void Update()
    {
        if (!initialized) return;
        if (body == null) return;
        
        if(!(body.stateMachine.currentState is AttackState)) return;
        // if (body.IsDead)
        // {
        //     ClearSpawnSettings();
        //     return;
        // }

        for (int i = 0; i < spawnSettingsList.Count; i++)
        {
            var setting = spawnSettingsList[i];
            if (setting.IsActive.Equals(0))
            {
                setting.SPON_TIME -= Time.deltaTime;
                if (setting.SPON_TIME <= 0f)
                {
                    setting.IsActive = 1;
                    var enemys = enemySpawnManager.SpawnEnemy(setting.MON_ID, setting.SPON_COUNT);
                    EnemysSetSpawnPoint(enemys, setting.SPON_POINT);
                }
                continue;
            }

            if (!setting.IsActive.Equals(1)) continue;

            setting.INTERVAL -= Time.deltaTime;
            Debug.Log($"{setting.MON_ID} Spawn Interval: {setting.INTERVAL}");
            if (setting.INTERVAL <= 0f)
            {
                var enemys = enemySpawnManager.SpawnEnemy(setting.MON_ID, setting.SPON_COUNT);
                EnemysSetSpawnPoint(enemys, setting.SPON_POINT);
                if (TryGetInterval(i, out float interval))
                {
                    Debug.Log($"{setting.MON_ID} Reset Spawn Interval: {interval}");
                    setting.INTERVAL = interval;
                    Debug.Log($"Setting Interval: {setting.INTERVAL}, Reset Spawn Interval: {interval}");
                }
            }
        }
    }

    private bool TryGetInterval(int index, out float interval)
    {
        if (index >= 0 && index < spawnIntervals.Count)
        {
            interval = spawnIntervals[index];
            return true;
        }
        interval = 0f;
        return false;
    }

    private void EnemysSetSpawnPoint(List<Enemy> enemys, int spawnPoint)
    {
        if (waveManager == null) return;
        foreach (var enemy in enemys)
        {
            enemy.transform.position = waveManager.SpawnPoints[spawnPoint] + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            enemy.move?.Init(enemy);    
        }
    }

    private bool SetSpawnSettings(int bossId)
    {
        var table = DataTableManager.BounsSpawnEnemys;
        if (table == null) return false;
        if (table.GetData(bossId) == null) return false;
        spawnGettingsList = table.GetData(bossId);
        return true;
    }

    public List<BounsSpawnEnemysTable.Data> GetSpawnSettingsList()
    {
        return spawnGettingsList;
    }

    public void ClearSpawnSettings()
    {
        spawnGettingsList.Clear();
        spawnSettingsList.Clear();
        spawnIntervals.Clear();
        this.enabled = false;
        initialized = false;
    }
}
