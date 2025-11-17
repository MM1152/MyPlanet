using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class WaveData : DataTable
{
    private Dictionary<int, Data> waveTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        public int Monster_ID { get; set; }
        public int Max_Spawn { get; set; }
        public int Spon { get; set; }
        public float Spon_Time { get; set; }
        public float Spon_Cycle { get; set; } 
    }
    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            waveTable.Add(data.ID, data);
        }

        return (filename, this as DataTable);
    }

    public Data GetData(int id)
    {
        if (!waveTable.ContainsKey(id))
        {
#if DEBUG_MODE
            throw new System.Exception($"WaveData  ID: {id} is not found.");
#endif
        }
        return waveTable[id];
    }

    public Dictionary<int, Data> GetAllData()
    {
        return waveTable;
    }
}
