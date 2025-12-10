using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ShopTable : DataTable
{
    private Dictionary<int, Data> shopTable = new Dictionary<int, Data>();
    public class Data
    {
        public int Tower_ID { get; set; }
        public int Price { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var result = await LoadCSV<Data>(textAssets.text);

        foreach (var data in result)
        {
            shopTable.Add(data.Tower_ID, data);
        }

        return (filename, this);
    }

    public List<Data> GetAllData()
    {
        return shopTable.Values.ToList();
    }
}