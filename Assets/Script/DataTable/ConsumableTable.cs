using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ConsumalbeTable : DataTable
{
    private Dictionary<int, Data> consumableTable = new Dictionary<int, Data>();
    public ConditionFactory conditionFactory = new ConditionFactory();
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

        [CsvHelper.Configuration.Attributes.Ignore]
        public ICondition condition;
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
            data.condition = conditionFactory.CreateInstance(data.Item_Condition);
        }

        return (filename, this);
    }

    public Data Get(int itemId)
    {
        return consumableTable[itemId];
    }

    public List<Data> GetAll()
    {
        return new List<Data>(consumableTable.Values);
    }

    public List<Data> GetDatasWithCondition(List<Tower> towers)
    {
        var lists = new List<Data>();

        foreach(var consumable in consumableTable.Values)
        {
            foreach(var tower in towers)
            {
                if(consumable.condition.CheckCondition(tower , null, null) && !lists.Contains(consumable))
                {
                    lists.Add(consumable);
                }
            }
        }

        return lists;
    }
}