using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using Unity.Android.Gradle.Manifest;
using System.Threading.Tasks;

public class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    //기존에 있던 테이블 코드
    public static CrewRankTable CrewRankTable
    {
        get
        {
            return Get<CrewRankTable>(DataTableIds.CrewRankTable);
        }
    }
    // 그씬에 필요한 테이블들을 한번에 비동기로 로드하는 메서드   
    public static async UniTask LoadAllAsync()
    {
        var crewRankTable = new CrewRankTable();
        var tasks = new List<UniTask<(string id, DataTable table)>>
        {
            crewRankTable.LoadAsync(DataTableIds.CrewRankTable),
        };

        var datas = await UniTask.WhenAll(tasks);
        Debug.Log("모든 테이블 로드 완료");

        foreach (var data in datas)
        {
            tables.Add(data.id, data.table);
            Debug.Log($"Loaded DataTable: {data.id}");
        }
        Debug.Log($"테이블 갯수: {tables.Count}");
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
