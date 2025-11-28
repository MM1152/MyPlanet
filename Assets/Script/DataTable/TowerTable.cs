using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TowerTable : DataTable
{
    private Dictionary<int, Data> towerTable = new Dictionary<int, Data>();

    public class Data
    {
        public int ID { get; set; }
        [Name("Name")]
        public int name { get; set; }
        public int Type { get; set; }
        public int ATK_Type { get; set; }
        public int Option_type { get; set; }
        public int Option_Range { get; set; }
        public int Attribute { get; set; }
        public int ATK { get; set; }
        public float Fire_Rate { get; set; }
        public string Image_path { get; set; }
        public string Bullet_path { get; set; }
        public int Option { get; set; }
        public float Min_Value { get; set; }
        public float Max_Value { get; set; }
        public float Attack_Range { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public string Name => DataTableManager.StringTable.Get(name);
        [CsvHelper.Configuration.Attributes.Ignore]
        public string AttackType => ATK_Type switch
        {
            1 => "레이저",
            2 => "기관총",
            3 => "샷건",
            4 => "미사일",
            5 => "저격",
            6 => "폭탄",
            7 => "기뢰",
            8 => "방전",
            _ => "정의되지 않음"
        };
        [CsvHelper.Configuration.Attributes.Ignore]
        public float optionValue;
        [CsvHelper.Configuration.Attributes.Ignore]
        public float FullOptionValue => optionValue;
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            data.optionValue = (int)Random.Range(data.Min_Value , data.Max_Value);
            towerTable.Add(data.ID, data);
        }

        return (filename, this as DataTable);
    }

    public Data Get(int id)
    {
        if (!towerTable.ContainsKey(id))
        {
#if DEBUG_MODE
            //throw new System.Exception($"TowerData 에는 ID {id} 가 존재하지 않습니다.");
#endif
            return null;
        }

        return towerTable[id];
    }

    public int Count()
    {
        return towerTable.Count;
    }

    public List<Data> GetAll()
    {
        return new List<Data>(towerTable.Values);
    }
}
