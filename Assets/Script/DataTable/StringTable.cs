using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StringTable : DataTable
{
    private Dictionary<int , Data> stringTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        public string String_kor { get; set; }
        public string String_en { get; set; }
        public string String_cn { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var csvText = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var records = await LoadCSV<Data>(csvText.text);

        foreach (var record in records)
        {
            stringTable.Add(record.ID, record);
        }

        return (filename, this);
    }

    public string Get(int id)
    {
        return stringTable[id].String_kor;
    }
}
