using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class WaveData : DataTable
{
     private Dictionary<int, Data> waveTable = new Dictionary<int, Data>();
    public class Data
    {
        public int WaveID { get; set; }
        public int EnemyID { get; set; }
        public int Count { get; set; }
        public float SpawnInterval { get; set; }
        public float SpawnDelay { get; set; }   
        public int SpawnPoint { get; set; }   
       public int Time { get; set;  }

    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            waveTable.Add(data.WaveID, data);            
        }

        return (filename, this as DataTable);
    }
    public Data GetData(int id)
    {
        if (!waveTable.ContainsKey(id))
        {
            return null;
        }        
        return waveTable[id];
    }
}
