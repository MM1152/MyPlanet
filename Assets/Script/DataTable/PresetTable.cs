using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PresetTable : DataTable
{
    private List<Data> towerIds;

    [Serializable]
    public class Data : JsonSerialized
    {
        public string PresetName;
        public int PlanetId;
        public List<int> TowerId;
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(string.Format(FormatPath, filename)).ToUniTask();
        towerIds = JsonConvert.DeserializeObject<List<Data>>(textAsset.text);
        
        return (filename, this as DataTable);
    }

    public Data Get(int index)
    {
        return towerIds[index];
    }

    public async UniTask Save()
    {
        string path = Application.dataPath + "/DataTable/PresetTable.json";
        if (File.Exists(path))
        {
            var json = JsonConvert.SerializeObject(towerIds, Formatting.Indented);
            File.WriteAllText(path, json);
            await LoadAsync(DataTableIds.PresetTable);
        }
    }

    public int Count()
    {
        return towerIds.Count;
    }
}

