using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;

public class PlanetPassiveLevelTable : DataTable
{
    private Dictionary<(int planetId , int grade) , Data> passiveLevelTable = new Dictionary<(int planetId, int grade), Data>();
    public class Data
    {
        public int ID { get; set; }
        public int Planet_ID { get; set; }
        public int Grade { get; set; }
        public int Value { get; set; }
        public int Time { get; set; }
        public int Cool_Time { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path);

        var result = await LoadCSV<Data>(textAssets.text);

        foreach (var data in result)
        {
            passiveLevelTable.Add((data.Planet_ID , data.Grade), data);
        }

        return (filename, this);
    }

    public Data Get((int planetId , int grade) id)
    {
        if(passiveLevelTable.ContainsKey(id))
        {
            return passiveLevelTable[id];
        }

        return null;
    }
}