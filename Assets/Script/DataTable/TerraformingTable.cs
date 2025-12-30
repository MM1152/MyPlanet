using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class TerraformingTable : DataTable
{
    private Dictionary<int, Data> terraformingTable = new Dictionary<int, Data>();
    private Dictionary<int, List<Data>> terraformingPointTable = new Dictionary<int, List<Data>>();

    public class Data
    {
        public int Terra_ID { get; set; }
        public int Terra_name { get; set; }
        public int unlock_point { get; set; }
        public int T_Effect_type { get; set; }
        public float T_effect_value { get; set; }
        public int T_effct_target { get; set; }
        public int T_description { get; set; }
        public string Image_path { get; set; }
        [CsvHelper.Configuration.Attributes.Ignore]
        public Sprite T_image { get; set; } 
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            terraformingTable.Add(data.Terra_ID, data);
            if (terraformingPointTable.ContainsKey(data.unlock_point))
            {
                terraformingPointTable[data.unlock_point].Add(data);
            }
            else
            {
                terraformingPointTable[data.unlock_point] = new List<Data> { data };
            }
             data.T_image = await Addressables.LoadAssetAsync<Sprite>(data.Image_path).ToUniTask();
        }

        return (filename, this as DataTable);
    }
    public Data GetData(int id)
    {
        if (!terraformingTable.ContainsKey(id))
        {
#if DEBUG_MODE
            throw new System.Exception($"TerraformingData  ID: {id} is not found.");
#endif
        }
        return terraformingTable[id];
    }

    public List<Data> GetDataByPoint(int point)
    {
        if (!terraformingPointTable.ContainsKey(point))
        {
#if DEBUG_MODE
            throw new System.Exception($"TerraformingData  Point: {point} is not found.");
#endif           
        }

        if (terraformingPointTable[point].Count != 2)
        {
#if DEBUG_MODE
            throw new System.Exception($"TerraformingData  Point: {point} does not have 2 data.");
#endif
        }

        return terraformingPointTable[point];
    }
}