using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ItemTable : DataTable
{
    private Dictionary<int, Data> itemTable = new Dictionary<int, Data>();

    public class Data
    {
        public int Item_ID { get; set; }
        public int? Item_Name { get; set; }
        public int? Item_Type { get; set; }
        public int? Category { get; set; }
        public int? Stack_Limit { get; set; }
        public int? sell_Availability { get; set; }
        public int? Sell_Value { get; set; }
        public string Icon_Path { get; set; }
        public string Description { get; set; }
            
        [CsvHelper.Configuration.Attributes.Ignore]
        public string Name => DataTableManager.StringTable.Get(Item_Name ?? 0);
        [CsvHelper.Configuration.Attributes.Ignore]
        public Sprite ItemImage { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path);
        var result = await LoadCSV<Data>(textAsset.text);

        foreach(var data in result)
        {
            itemTable.Add(data.Item_ID, data);
            if(!string.IsNullOrEmpty(data.Icon_Path))
            {
                data.ItemImage = await Addressables.LoadAssetAsync<Sprite>(data.Icon_Path).ToUniTask();
            }
        }

        return (filename, this);
    }   

    public Data Get(int id)
    {
        return itemTable[id];
    }
}
