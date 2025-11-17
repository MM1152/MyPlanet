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
    public event Action OnChangeDatas;
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
        string path = Application.dataPath + "/DataTables/PresetTable.json";
        if (File.Exists(path))
        {
            var json = JsonConvert.SerializeObject(towerIds, Formatting.Indented);
            File.WriteAllText(path, json);
            for(int i = 0; i < towerIds[0].TowerId.Count; i++)
            {
                Debug.Log(towerIds[0].TowerId[i]);
            }
            await LoadAsync(DataTableIds.PresetTable);
            Debug.Log("Save 이후");
            for (int i = 0; i < towerIds[0].TowerId.Count; i++)
            {
                Debug.Log(towerIds[0].TowerId[i]);
            }
            OnChangeDatas?.Invoke();
        }
    }

    public int Count()
    {
        return towerIds.Count;
    }
}

