using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlanetTable : DataTable
{
    private Dictionary<int , Data> planetTable = new Dictionary<int, Data>();

    public class Data 
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public int Explanation { get; set; }
        public string Rescoce_ID { get; set; }
        public char Grade { get; set; }
        public int Planet_type { get; set; }
        public int Attribute { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
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
        return planetTable[id];
    }
}
