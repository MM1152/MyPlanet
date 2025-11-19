using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EffectTable : DataTable
{
    private Dictionary<int, Data> effectTable = new Dictionary<int, Data>();

    public class Data
    {
        public int ID { get; set; }
        public int Effect_code { get; set; }
        public int Effect_Cycle { get; set; }
        public int Effect_Persent { get; set; }
        public int Stackable { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(textAssets.text);

        foreach (var data in datas)
        {
            effectTable.Add(data.ID, data);
        }

        return (filename, this as DataTable);
    }

    public Data Get(int id)
    {
        if (!effectTable.ContainsKey(id))
        {
            return null;
        }

        return effectTable[id];
    }

    public int Count()
    {
        return effectTable.Count;
    }

    public List<Data> GetAll()
    {
        return new List<Data>(effectTable.Values);
    }
}