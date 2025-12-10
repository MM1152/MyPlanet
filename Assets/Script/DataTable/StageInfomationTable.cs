using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections.Generic;

public class StageInfomationTable : DataTable
{
    private Dictionary<int, Data> stageTable = new Dictionary<int, Data>();
    public class Data
    {
        public int STAGE_ID { get; set; }
        public string STAGE_NAME { get; set; }
        public float STAGE_TIME { get; set; }
        public int WAVE_COUNT { get; set; }
        public float EXP_MULTIPLES { get; set; }
        public float DIFFICULTY_MULTIPLES { get; set; }
        public int CLEAR_REWARD1 { get; set; }
        public int CLEAR_REWARD1_COUNT { get; set; }
        public int CLEAR_REWARD2 { get; set; }
        public int CLEAR_REWARD2_COUNT { get; set; }
        public int CLEAR_REWARD3 { get; set; }
        public int CLEAR_REWARD3_COUNT { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            stageTable.Add(data.STAGE_ID, data);
        }

        return (filename, this as DataTable);
    }
 
    public Data Get(int stageIndex)
    {
        int stageId = 22000 + stageIndex;
        if(stageTable.ContainsKey(stageIndex))
        {
            return stageTable[stageId];
        }
        return null;
    }
}