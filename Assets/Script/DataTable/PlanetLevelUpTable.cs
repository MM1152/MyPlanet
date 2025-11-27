using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlanetLevelUpTable : DataTable
{
    private Dictionary<(int , int) , Data> levelupTable = new Dictionary<(int, int), Data>();
    public class Data
    {
        public int ID { get; set; }
        public int Planet_ID { get; set; }
        public int LV { get; set; }
        [CsvHelper.Configuration.Attributes.Name("HP")]
        public string hp { get; set; }
        [CsvHelper.Configuration.Attributes.Name("ATK")]
        public string atk { get; set; }
        [CsvHelper.Configuration.Attributes.Name("DEF")]
        public string def { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Exp")]
        public string exp { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Gold")]
        public string gold { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public int HP => int.Parse(hp.Replace(",", ""));
        [CsvHelper.Configuration.Attributes.Ignore]
        public int ATK => int.Parse(atk.Replace(",", ""));
        [CsvHelper.Configuration.Attributes.Ignore]
        public int DEF => int.Parse(def.Replace(",", ""));
        [CsvHelper.Configuration.Attributes.Ignore]
        public int Exp => int.Parse(exp.Replace(",", ""));
        [CsvHelper.Configuration.Attributes.Ignore]
        public int Gold => int.Parse(gold.Replace(",", ""));
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path);

        var result = await LoadCSV<Data>(textAssets.text);

        foreach(var data in result)
        {
            //if(levelupTable.ContainsKey((data.Planet_ID, data.LV)))
            //{
            //    Debug.Log($"Key ม฿บน {(data.Planet_ID, data.LV)}");
            //    continue;
            //}
            levelupTable.Add((data.Planet_ID, data.LV), data);
        }

        return (filename, this);
    }

    public Data GetData((int planetId, int level) id)
    {
        if (!levelupTable.ContainsKey(id))
        {
            return null;
        }
        return levelupTable[id];
    }

    public Data GetData(int planetId , int level)
    {
        if(!levelupTable.ContainsKey((planetId, level)))
        {
            return null;
        }
        return levelupTable[(planetId, level)];
    }
}