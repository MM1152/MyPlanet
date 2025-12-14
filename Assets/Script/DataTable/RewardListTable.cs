using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class RewardListTable : DataTable
{
    private Dictionary<int, Data> rewardListTable = new Dictionary<int, Data>();

    public class Data
    {
        public int Rewerd_ID { get; set; }
        public int? item_ID { get; set; }
        public int? item_name { get; set; }
        public int value { get; set; }
        public int Stackable { get; set; }
        public int Stackable_Max { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var result = await LoadCSV<Data>(textAsset.text);
        foreach (var data in result)
        {
            rewardListTable.Add(data.Rewerd_ID, data);
        }
        return (filename, this);
    }

    public Data GetData(int rewardListId)
    {
        return rewardListTable[rewardListId];
    }

    public Data Get(int reward_Id)
    {
        return rewardListTable[reward_Id];
    }
}