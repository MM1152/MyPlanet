using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TowerGradeToPieceCountTable : DataTable
{
    private Dictionary<int , Data> dataTable = new Dictionary<int, Data>();
    public class Data
    {
        public int PROMOTION_TOWER_ID { get; set; }
        public int INGREDITEM1 { get; set; }
        public int INGREDITEM_COUNT1 { get; set; }
        public int INGREDITEM2 { get; set; }
        public int INGREDITEM2_COUNT { get; set; }
        public int MIN_VALUE { get; set; }
        public int MAX_VALUE { get; set; }
    }


    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAseets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var result = await LoadCSV<Data>(textAseets.text);

        foreach(var data in result)
        {
            dataTable.Add(data.PROMOTION_TOWER_ID, data);
        }

        return (filename , this);
    }
    
    private Data Get(int id)
    {
        return dataTable[id];
    }

    public int GetPieceCount(int towerId , int grade)
    {
        int id = int.Parse("30" + towerId.ToString() + grade.ToString());

        return Get(id).INGREDITEM2_COUNT;
    }
}