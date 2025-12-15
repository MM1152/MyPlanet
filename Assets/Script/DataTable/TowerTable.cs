using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TowerTable : DataTable
{
    private Dictionary<int, Data> towerTable = new Dictionary<int, Data>();

    public class UtilTower : Data
    {
        public int Tower_ID { get; set; }
        public int Tower_Name_ID { get; set; }
        public int drone_recall { get; set; }
        public int drone_count { get; set; }
        public int drone_hp { get; set; }
        public int Effect_Type { get; set; }
        public int Target_Type { get; set; }
        public int Range_Shape { get; set; }
        public int range { get; set; }
        public float Duration { get; set; }
        public float Cooltime { get; set; }
        public int Description { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public override string Name => DataTableManager.StringTable.Get(Tower_Name_ID);
        [CsvHelper.Configuration.Attributes.Ignore]
        public override int ID => Tower_ID;
        [CsvHelper.Configuration.Attributes.Ignore]
        public override EffectTable.Data Effect => DataTableManager.EffectTable.Get(Effect_Type);
        [CsvHelper.Configuration.Attributes.Ignore]
        public override string Explanatoin => DataTableManager.StringTable.Get(Description);
        [CsvHelper.Configuration.Attributes.Ignore]
        public override int Type => 2;
    }


    public class Data
    {
        [Name("ID")]
        public int id { get; set; }
        [Name("Name")]
        public int name { get; set; }
        [Name("Type")]
        public int type { get; set; }
        public int ATK_Type { get; set; }
        public int Option_type { get; set; }
        public int Option_Range { get; set; }
        [Name("Attribute")]
        public int attribute { get; set; }
        public int ATK { get; set; }
        public float Fire_Rate { get; set; }
        public string Image_path { get; set; }
        public string Bullet_path { get; set; }
        public int Option { get; set; }
        public float Min_Value { get; set; }
        public float Max_Value { get; set; }
        public float Attack_Range { get; set; }
        [Name("Buff_Explantion")]
        public int buff_Explantion { get; set; }
        [Name("Explanation")]
        public int explanation { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public virtual int ID => id;
        [CsvHelper.Configuration.Attributes.Ignore]
        public virtual string Name => DataTableManager.StringTable.Get(name);
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
        public Sprite towerImage;
        [CsvHelper.Configuration.Attributes.Ignore]
        public virtual int Type => type;
        [CsvHelper.Configuration.Attributes.Ignore]
        public virtual string Explanatoin => DataTableManager.StringTable.Get(explanation);
        [CsvHelper.Configuration.Attributes.Ignore]
        public string Buff_Explanation => string.Format(DataTableManager.StringTable.Get(buff_Explantion) , OptionValue);
        [CsvHelper.Configuration.Attributes.Ignore]
        public string TypeToString => type switch
        {
            1 => "공격",
            2 => "유틸",
            _ => "정의되지 않음"
        };
        [CsvHelper.Configuration.Attributes.Ignore]
        public string AttributeToString => attribute switch
        {
            1 => "불",
            2 => "얼음",
            3 => "금속",
            4 => "빛",
            5 => "어둠",
            _ => "정의되지 않음"
        };
        [CsvHelper.Configuration.Attributes.Ignore]
        public (Color outlineColor, Color backGroundColor) AttributeToColor => attribute switch
        {
            3 => (new Color(0xA5/255f, 0xC1/255f, 0xBB/255f, 1f), new Color(0x4B/255f, 0x4B/255f, 0x4B/255f, 1f)), // 금속 속성
            1 => (new Color(0xFF/255f, 0x00/255f, 0x00/255f, 1f), new Color(0x6F/255f, 0x1B/255f, 0x1B/255f, 1f)), // 불 속성
            2 => (new Color(0x64/255f, 0xCB/255f, 0xFF/255f, 1f), new Color(0x1A/255f, 0x6C/255f, 0xA6/255f, 1f)), // 냉기 속성
            4 => (new Color(0xFF/255f, 0xCE/255f, 0x00/255f, 1f), new Color(0x87/255f, 0x7C/255f, 0x40/255f, 1f)), // 빛 속성
            5 => (new Color(0xC7/255f, 0xA3/255f, 0xFF/255f, 1f), new Color(0x5C/255f, 0x38/255f, 0x95/255f, 1f)), // 어둠 속성
            _ => (Color.blue , Color.blue)
        };
        [CsvHelper.Configuration.Attributes.Ignore]
        public Sprite TypeImage => DataTableManager.SpriteTable.Get(DataTableIds.TypeSpriteTable, Type);
        [CsvHelper.Configuration.Attributes.Ignore]
        public Sprite AttackTypeImage => DataTableManager.SpriteTable.Get(DataTableIds.AttackTypeSpriteTable, ATK_Type);
        [CsvHelper.Configuration.Attributes.Ignore]
        public Sprite ElementImage => DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, attribute);

        [CsvHelper.Configuration.Attributes.Ignore]
        public float OptionValue => FirebaseManager.Instance.TowerData.GetOptionValue(ID);
        [CsvHelper.Configuration.Attributes.Ignore]
        public bool Unlock => FirebaseManager.Instance.TowerData.IsUnlocked(ID);
        [CsvHelper.Configuration.Attributes.Ignore]
        public virtual EffectTable.Data Effect => null;
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            towerTable.Add(data.ID, data);
            data.towerImage = await Addressables.LoadAssetAsync<Sprite>(data.Image_path).ToUniTask();
        }

        return (filename, this as DataTable);
    }

    public async UniTask<(string, DataTable)> LoadUtilTowerAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<UtilTower>(textAsset.text);
        foreach (var data in datas)
        {
            towerTable.Add(data.Tower_ID, data);
            data.towerImage = await Addressables.LoadAssetAsync<Sprite>(data.Image_path).ToUniTask();
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
