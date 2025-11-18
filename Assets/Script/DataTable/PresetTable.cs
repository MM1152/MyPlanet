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
    private Data inGameData;

    public event Action<int> OnChangePresetData;

    [Serializable]
    public class Data : JsonSerialized
    {
        public string PresetName;
        public int PlanetId;
        public List<int> TowerId;
    }


    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        // 추후에 파이어 베이스 연동으로 변경
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(string.Format(FormatPath, filename)).ToUniTask();
        towerIds = JsonConvert.DeserializeObject<List<Data>>(textAsset.text);
        
        return (filename, this as DataTable);
    }

    public void SetGameData(Data InGameData)
    {
        this.inGameData = InGameData;
    } 

    public Data GetGameData()
    {
        return inGameData;
    }

    public Data Get(int index)
    {
        Data copyData = new Data()
        {
            PresetName = towerIds[index].PresetName,
            PlanetId = towerIds[index].PlanetId,
            TowerId = new List<int>(towerIds[index].TowerId)
        };
        return copyData;
    }

    public async UniTask<(bool sucess , string msg)> Save(Data changedData , int index)
    {
        // 추후에 파이어 베이스 연동으로 변경
        towerIds[index] = changedData;

        string path = Application.dataPath + "/DataTables/PresetTable.json";
        if (File.Exists(path))
        {
            var json = JsonConvert.SerializeObject(towerIds, Formatting.Indented);
            File.WriteAllText(path, json);
            UnityEditor.AssetDatabase.Refresh();
            await LoadAsync(DataTableIds.PresetTable);
            OnChangePresetData?.Invoke(index);

            return (true, "성공적으로 저장완료");
        }
        return (false, "파일이 존재하지 않음");
    }

    public int Count()
    {
        return towerIds.Count;
    }
}

