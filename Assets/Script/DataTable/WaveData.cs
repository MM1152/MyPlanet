using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class WaveData : DataTable
{
    private Dictionary<int, List<Data>> waveTable = new Dictionary<int, List<Data>>();

    private Dictionary<int, StageData> stageDatas = new Dictionary<int, StageData>();


    public class Data
    {
        public int ID { get; set; }
        public int MON_ID { get; set; }
        //스폰까지의 시간
        public float SPON_TIME { get; set; }
        //한번에 소환될 몬스터 수
        public int SPON_COUNT { get; set; }
        public float INTERVAL { get; set; }
        public int SPON_POINT { get; set; }
        public int MAX_SPON { get; set; }
    }
    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            int waveNumber = data.ID % 1000;
            int stageNumber = (data.ID / 1000) % 1000;

            if (!stageDatas.TryGetValue(stageNumber, out var stageData))
            {
                stageData = new StageData()
                {
                    stageId = stageNumber
                };
                stageDatas.Add(stageNumber, stageData);
            }

            var waveGroup = stageData.waveGroups.Find(wg => wg.waveIndex == waveNumber);

            if (waveGroup == null)
            {
                waveGroup = new WaveGroup()
                {
                    stageId = stageNumber,
                    waveIndex = waveNumber,
                };
                stageData.waveGroups.Add(waveGroup);
            }

            waveGroup.waveDatas.Add(data);

            if (!waveTable.ContainsKey(data.ID))
            {
                waveTable[data.ID] = new List<Data>();
            }
            waveTable[data.ID].Add(data);
        }

        return (filename, this as DataTable);
    }

    public List<Data> GetData(int id)
    {
        if (!waveTable.ContainsKey(id))
        {
#if DEBUG_MODE
            throw new System.Exception($"WaveData  ID: {id} is not found.");
#endif
        }
        return waveTable[id];
    }

    public StageData GetStageData(int stageId)
    {
        if (!stageDatas.ContainsKey(stageId))
        {
#if DEBUG_MODE
            Debug.LogError($"StageData  ID: {stageId} is not found.");
#endif
            return null;
        }
        return stageDatas[stageId];
    }

    public Dictionary<int, List<Data>> GetAllData()
    {
        return waveTable;
    }
}
