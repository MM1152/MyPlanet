using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class DailyGiftTable : DataTable
{
    private Dictionary<int , Data> dailyGiftTable = new Dictionary<int, Data>();

    public class Data
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int Num { get; set; }

        [Ignore]
        public ItemTable.Data ItemData => DataTableManager.ItemTable.Get(Type);
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path);
        var result = await LoadCSV<Data>(textAssets.text);

        foreach(var data in result)
        {
            dailyGiftTable.Add(data.ID, data);
        }

        return (filename, this);
    }

    public List<Data> Get()
    {
        return dailyGiftTable.Values.ToList();
    }
}