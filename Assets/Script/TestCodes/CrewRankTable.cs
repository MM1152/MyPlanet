using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CrewRankTable : DataTable
{
    public class Data
    {
        public int rank_ID { get; set; }
        public string name { get; set; }
        public string Color { get; set; }
        public int Buyround { get; set; }

        public Color GetColor => Color switch
        {
            "Green" => new Color(0.3118286f, 0.5396226f, 0f, 1f),
            "Blue" => new Color(0.01666641f, 0f, 1f, 1f),
            "Purple" => new Color(1f, 0.6229727f, 0f, 1f),
            "Yellow" => new Color(0.3660378f, 0.2492808f, 0.07666072f, 1f)
        };
    }

    private Dictionary<int, Data> rankTable = new Dictionary<int, Data>();

  //리소스로딩이아닌 어드레서블로 로딩으로 변경

    public override async UniTask<(string , DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var ReAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var textAsset = ReAssets as TextAsset;
        var datas =  await LoadCSVTest<Data>(textAsset.text);

        foreach (var data in datas)
        {
            rankTable.Add(data.rank_ID, data);
        }

        return (filename , this as DataTable);
    }
    public Data Get(int rank_Id)
    {
        return rankTable[rank_Id];
    }

}