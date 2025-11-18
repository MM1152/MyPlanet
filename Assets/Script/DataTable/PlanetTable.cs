using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlanetTable : DataTable
{
    private Dictionary<int, Data> planetTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        [Name("Name")]
        public int name { get; set; }
        [Name("Explanation")]
        public int explanation { get; set; }
        public string Rescoce_ID { get; set; }  
        public string grade { get; set; }
        [Name("Planet_type")]
        public int planet_type { get; set; }
        public int Attribute { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int Skill_ID { get; set; }

        public string Name => DataTableManager.StringTable.Get(name);
        public string Explanation => DataTableManager.StringTable.Get(explanation);
        public string PlanetType => planet_type switch
        {
            1 => "암석형",
            2 => "가스형",
            3 => "왜소행성",
            _ => "정의되지 않음"
        };
        public string AttributeType => Attribute switch
        {
            1 => "불",
            2 => "냉기",
            3 => "금속",
            4 => "빛",
            5 => "어둠",
            _ => "정의되지 않음"
        };
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(textAssets.text);

        for(int i = 0; i < datas.Count; i++)
        {
            planetTable.Add(datas[i].ID, datas[i]);
        }

        return (filename, this as DataTable);
    }

    public Data Get(int id)
    {
        if(!planetTable.ContainsKey(id))
        {
            return null;
        }
        return planetTable[id];
    }

    public List<Data> GetAllData()
    {
        return new List<Data>(planetTable.Values);
    }
}
