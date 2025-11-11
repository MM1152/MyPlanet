using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;



public class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();


    // 그씬에 필요한 테이블들을 한번에 비동기로 로드하는 메서드   
    public static async UniTask LoadAllAsync()
    {
        var enemyDatatable = new EnemyData();
        var tasks = new List<UniTask<(string id, DataTable table)>>
        {
             enemyDatatable.LoadAsync(DataTableIds.EnemyTable),
        };

        var datas = await UniTask.WhenAll(tasks);
        

        foreach (var data in datas)
        {
            tables.Add(data.id, data.table);            
        }
        
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
}
