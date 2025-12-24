using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ClearWaveRewardTable : DataTable
{
    private Dictionary<int, Data> rewardTable = new Dictionary<int, Data>();

    public class Data
    {
        public int ID { get; set; }
        public int Wave { get; set; }
        public int Type { get; set; }
        public int Num { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var result = await LoadCSV<Data>(textAssets.text);

        foreach(var data in result)
        {
            rewardTable.Add(data.ID , data);
        }

        return (filename, this);
    }

    public Data Get(int id)
    {
        return rewardTable[id];
    }
}