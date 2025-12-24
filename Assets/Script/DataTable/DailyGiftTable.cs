using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class DailyGiftTable : DataTable
{
    private List<Data> dailyGiftTable = new List<Data>();
    public class Data
    {
        public int ID { get; set; }
        public int Type { get; set; } 
        public int Num { get; set; }

        [Ignore]
        public Sprite ItemImage { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path);

        var result = await LoadCSV<Data>(textAsset.text);

        foreach(var data in result)
        {
            dailyGiftTable.Add(data);
        }

        return (filename, this);
    }

    public List<Data> GetDailyGiftDatas() 
    {
        return dailyGiftTable;
    }
}