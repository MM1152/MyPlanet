using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BounsSpawnEnemysTable : DataTable
{
    private Dictionary<int, List<Data>> bossSpawnTable = new Dictionary<int, List<Data>>();

    public class Data
    {
        public int BOS_ID { get; set; }
        public int MON_ID { get; set; }
        public int IsActive { get; set; }
        public float SPON_TIME { get; set; }
        public int SPON_COUNT { get; set; }
        public float INTERVAL { get; set; }
        public int SPON_POINT { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            int bossId = data.BOS_ID;

            if (!bossSpawnTable.ContainsKey(bossId))
            {
                bossSpawnTable[bossId] = new List<Data>();
            }

            bossSpawnTable[bossId].Add(data);
        }

        return (filename, this);
    }

    public List<Data> GetData(int id)
    {
        if (!bossSpawnTable.ContainsKey(id))
        {
            return null;
        }
        return bossSpawnTable[id];
    }

    public Dictionary<int, List<Data>> GetAllData()
    {
        return bossSpawnTable;
    }
}
