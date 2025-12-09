using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BasePlanetLevelTable : DataTable
{
    private Dictionary<int, Data> levelTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        public int LEVEL { get; set; }
        public int Required_Exp { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var result = await LoadCSV<Data>(textAssets.text);

        foreach (var data in result)
        {
            levelTable.Add(data.LEVEL, data);
        }

        return (filename, this);
    }

    public int GetRequiredExp(int level)
    {
        if (levelTable.ContainsKey(level))
        {
            return levelTable[level].Required_Exp;
        }

        return -1;
    }
}