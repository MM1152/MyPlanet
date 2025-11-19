using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class PassiveTable : DataTable
{
    private Dictionary<int , Data> passiveTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        public int Condition { get; set; }
        public int Effect_Id { get; set; }
        public int Val { get; set; }
        public int Time { get; set; }
        public int Cool_Time { get; set; }
        public int Target { get; set; }
        public int Name { get; set; }
        public int Explanation { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var result = await LoadCSV<Data>(textAsset.text);

        for(int i = 0; i < result.Count; i++)
        {
            passiveTable.Add(result[i].ID, result[i]);
        }

        return (filename, this);
    }

    public Data GetData(int id)
    {
        if(passiveTable.TryGetValue(id, out var data))
        {
            return data;
        }
        return null;
    }
}
