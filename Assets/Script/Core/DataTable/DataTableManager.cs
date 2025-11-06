using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using Unity.Android.Gradle.Manifest;

public class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();


    //기존에 있던 테이블 코드
    // static DataTableManager()
    // {
    //     Init();
    // }

    // private static void Init()
    // {
    //     var crewRankTable = new CrewRankTable();
    //     crewRankTable.Load(DataTableIds.StringTableIds);
    //     tables.Add(DataTableIds.StringTableIds, crewRankTable);
    // }

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
        var tasks = new List<UniTask>
        {
            CrewRankTable.LoadAsync(DataTableIds.CrewRankTable),
        };

        await UniTask.WhenAll(tasks);
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
