using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class LevelUpTable : DataTable
{
    private readonly int tableId = 11000000;
    private Dictionary<int, Data> levelUpTable = new Dictionary<int, Data>();
    /*
        1   발사체 개수
        2	사거리
        3	두께(가로길이)
        4	지속시간
        5	쿨타임
        6	연사속도 
        7	필렛 개수
        8	파편사거리
        9	파편개수
        10	폭발 범위 
        11	유도횟수
        12	이동속도 감소량 
        13	총알 속도 감소량
        14	정지시간
        15	퍼지는 각도
        16	탄환 속도
    */
    public class Data
    {
        public int ID { get; set; }
        public int Tower_ID { get; set; }
        public int Damage { get; set; }
        public int LV { get; set; }

        public int Var1 { get; set; }
        public int Val1 { get; set; }

        public int Var2 { get; set; }
        public int Val2 { get; set; }

        public int Var3 { get; set; }
        public int Val3 { get; set; }

        public int Var4 { get; set; }
        public int Val4 { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path);

        var result = await LoadCSV<Data>(textAsset.text);

        foreach(var data in result)
        {
            levelUpTable.Add(data.ID, data);
        }

        return (filename, this);
    }

    public Data Get(int towerId , int level)
    {
        int id = tableId + towerId * 100 + level;
        return levelUpTable[id];
    }
}
