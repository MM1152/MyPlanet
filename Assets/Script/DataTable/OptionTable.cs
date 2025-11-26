using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class OptionTable : DataTable
{
    private Dictionary<int, Data> optionTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        public float Value { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path);

        var result = await LoadCSV<Data>(textAssets.text);
        foreach ( var data in result)
        {
            optionTable.Add(data.ID, data);
        }

        return (filename, this);
    }

    public Data GetData(int id)
    {
        return optionTable[id];
    }

    public int GetValueDataToInt(int id)
    {
        return (int)optionTable[id].Value;
    }

    public float GetValueDataToFloat(int id)
    {
        return optionTable[id].Value;
    }
}