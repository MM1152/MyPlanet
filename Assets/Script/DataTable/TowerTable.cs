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
        public string Name { get; set; }
        public string Explanation { get; set; }
        public string Grade { get; set; }
        public int Attribute { get; set; }
        public int ATK { get; set; }
        public int Range { get; set; }
        public string Bullet_Path { get; set; }
        public string Image_Path { get; set; }
        public int Fire_Rate { get; set; }
        public int Option { get; set; }
        public int Min_Value { get; set; }
        public int Max_Value { get; set; }
        public int Option_Type { get; set; }
        public int Option_Range { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public int optionValue;
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            data.optionValue = Random.Range(data.Min_Value , data.Max_Value);
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
