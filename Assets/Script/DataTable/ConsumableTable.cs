using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ConsumalbeTable : DataTable
{
    private Dictionary<int, Data> consumableTable = new Dictionary<int, Data>();

    public class Data
    {
        public int Item_id { get; set; }
        public int name_id { get; set; }
        public int Item_buff { get; set; }
        public int Item_Condition { get; set; }
        public int effect_id { get; set; }
        public float effect_value { get; set; }
        public int duration { get; set; }
        public int Item_description { get; set; }

        public string Name => DataTableManager.StringTable.Get(name_id);
        public string Description => DataTableManager.StringTable.Get(Item_description);
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var result = await LoadCSV<Data>(textAssets.text);

        foreach(var data in result)
        {
            consumableTable.Add(data.Item_id, data);
        }

        return (filename, this);
    }

    public Data Get(int itemId)
    {
        return consumableTable[itemId];
    }
}