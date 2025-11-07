using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;    

public class CrewRankTable : DataTable
{
    public class Data
    {
        public int ID { get; set; }
        public string name { get; set; }
    }

    private Dictionary<int, Data> rankTable = new Dictionary<int, Data>();
    // 리소드 동기로딩 
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAssets.text);

        foreach (var data in datas)
        {
            rankTable.Add(data.ID, data);
        }
    }

    // 어드레서블 비로딩
    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();        
        var datas = await LoadCSVTest<Data>(textAsset.text);

        foreach (var data in datas)
        {
            rankTable.Add(data.ID, data);
        }

        return (filename, this);
    }
    public Data Get(int rank_Id)
    {
        return rankTable[rank_Id];
    }

}