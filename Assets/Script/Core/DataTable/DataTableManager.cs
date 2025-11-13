using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;



public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();


    public static bool init = false;

    public static EnemyData EnemyTable => Get<EnemyData>(DataTableIds.EnemyTable);
    public static TowerTable TowerTable => Get<TowerTable>(DataTableIds.TowerTable);
    public static WaveData WaveTable => Get<WaveData>(DataTableIds.WaveTable);


    static DataTableManager()
    {
        LoadAllAsync().Forget();
    }
    // 그씬에 필요한 테이블들을 한번에 비동기로 로드하는 메서드   
    private static async UniTask LoadAllAsync()
    {
        var enemyDatatable = new EnemyData();
        var towerTable = new TowerTable();
        var waveTable = new WaveData();
        var tasks = new List<UniTask<(string id, DataTable table)>>
        {
            enemyDatatable.LoadAsync(DataTableIds.EnemyTable),
            towerTable.LoadAsync(DataTableIds.TowerTable),
            waveTable.LoadAsync(DataTableIds.WaveTable)
        };

        var datas = await UniTask.WhenAll(tasks);


        foreach (var data in datas)
        {
            tables.Add(data.id, data.table);
        }

        Debug.Log($"테이블 갯수: {tables.Count}");
        init = true;
    }

    //기존에 있던 테이블 코드
    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.Log("id not found");
            return null;
        }
        return tables[id] as T;
    }

    public static async UniTask WaitForInitalizeAsync()
    {
        await UniTask.WaitUntil(() => init);
    }
}
