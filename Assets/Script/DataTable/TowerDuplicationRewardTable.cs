using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class TowerDuplicationRewardTable : DataTable
{
    private Dictionary<(int towerId , int towerGrade), Data> towerDuplicationRewardTable = new Dictionary<(int towerId, int towerGrade), Data>();
    public class Data
    {
        public int id { get; set; }
        public int tower_id { get; set; }
        public int item_ID { get; set; }
        public int tower_grade { get; set; }
        public int L_piece_count { get; set; }
        public int M_piece_count { get; set; }
        public int H_piece_count { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var result = await LoadCSV<Data>(textAsset.text);

        foreach(var data in result)
        {
            towerDuplicationRewardTable.Add((data.tower_id, data.tower_grade), data);
        }

        return (filename, this);
    }
    // HML = 0 : LOW , 1 : MID , 2 : HIGH
    public int GetDuplicationPartCount(int towerId , int grade , int LMH)
    {
        var duplicationData = towerDuplicationRewardTable[(towerId , grade)];

        return LMH switch
        {
            0 => duplicationData.L_piece_count,
            1 => duplicationData.M_piece_count,
            2 => duplicationData.H_piece_count,
            _ => 0,
        };
    }
}