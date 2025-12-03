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
        public (Color outlineColor, Color backGroundColor) AttributeToColor => Attribute switch
        {
            3 => (new Color(0xA5/255f, 0xC1/255f, 0xBB/255f, 1f), new Color(0x4B/255f, 0x4B/255f, 0x4B/255f, 1f)), // 금속 속성
            1 => (new Color(0xFF/255f, 0x00/255f, 0x00/255f, 1f), new Color(0x6F/255f, 0x1B/255f, 0x1B/255f, 1f)), // 불 속성
            2 => (new Color(0x64/255f, 0xCB/255f, 0xFF/255f, 1f), new Color(0x1A/255f, 0x6C/255f, 0xA6/255f, 1f)), // 냉기 속성
            4 => (new Color(0xFF/255f, 0xCE/255f, 0x00/255f, 1f), new Color(0x87/255f, 0x7C/255f, 0x40/255f, 1f)), // 빛 속성
            5 => (new Color(0xC7/255f, 0xA3/255f, 0xFF/255f, 1f), new Color(0x5C/255f, 0x38/255f, 0x95/255f, 1f)), // 어둠 속성
            _ => (Color.white , Color.white)
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
