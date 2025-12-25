using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlanetTable : DataTable
{
    private Dictionary<int, Data> planetTable = new Dictionary<int, Data>();
    public GameObject Model { get; set; }
    public GameObject SubModel { get; set; }
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
        public string Image_Path { get; set; }

        public string Name => DataTableManager.StringTable.Get(name);
        public string Explanation => DataTableManager.StringTable.Get(explanation + 1);
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
        public float NeedPeiceCountPercent => grade switch
        {
            "C" => DataTableManager.OptionTable.GetValueDataToFloat(5098),
            "B" => DataTableManager.OptionTable.GetValueDataToFloat(5099),
            "A" => DataTableManager.OptionTable.GetValueDataToFloat(5100),
            "S" => DataTableManager.OptionTable.GetValueDataToFloat(5101),
            _ => 0f
        };
        public float InitOpenSlotCount => grade switch
        {
            "C" => DataTableManager.OptionTable.GetValueDataToInt(5002),
            "B" => DataTableManager.OptionTable.GetValueDataToInt(5003),
            "A" => DataTableManager.OptionTable.GetValueDataToInt(5004),
            "S" => DataTableManager.OptionTable.GetValueDataToInt(5005),
            _ => 0f
        };
        public Color GradeToColor => grade switch
        {
            "S" => new Color32(0xff, 0xbf, 0x00, 0xff), // #ffbf00
            "A" => new Color32(0xcc, 0xa8, 0xf7, 0xff), // #cca8f7
            "B" => new Color32(0x58, 0xcc, 0xff, 0xff), // #58ccff
            "C" => new Color32(0xaf, 0xd4, 0x85, 0xff), // #afd485
            _ => Color.white
        };
        [Ignore]
        public Sprite PlanetImage { get; set; }

    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(textAssets.text);

        for(int i = 0; i < datas.Count; i++)
        {
            planetTable.Add(datas[i].ID, datas[i]);
            datas[i].PlanetImage = await Addressables.LoadAssetAsync<Sprite>(datas[i].Image_Path).ToUniTask();
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

    public int GetUnlockAbleSlotCount(int planetId, int starCount)
    {
        var idx = 0;
        var grade = Get(planetId).grade;

        switch(grade) 
        {
            case "C":
                idx = 5041 + starCount;
                break;
            case "B":
                idx = 5047 + starCount;
                break;
            case "A":
                idx = 5053 + starCount;
                break;
            case "S":
                idx = 5059 + starCount;
                break;
        }

        return DataTableManager.OptionTable.GetValueDataToInt(idx);
    }

    public async UniTask LoadPlanetPrefab(int planetId)
    {
        var path = string.Format(AddressableFormatPaths.PlanetPrefabFormating , Get(planetId).Rescoce_ID);
        if(planetId == 1011)
        {
            var subPath = string.Format(AddressableFormatPaths.PlanetPrefabFormating, "Planet_12");
            SubModel = await Addressables.LoadAssetAsync<GameObject>(subPath).ToUniTask();
        }
        Model = await Addressables.LoadAssetAsync<GameObject>(path).ToUniTask();
    }

    public Data GetRandomPlaentData(char[] limitGrade)
    {
        var planetDatas = GetAllData();
        var limitPlanetDatas = planetDatas.Where(x =>
        {
            for(int i = 0; i < limitGrade.Length; i++)
            {
                if(x.grade == limitGrade[i].ToString())
                {
                    return true;
                }
            }
            return false;
        }).ToList();

        var randomIndex = Random.Range(0, limitPlanetDatas.Count());
        return limitPlanetDatas[randomIndex];
    }
}
