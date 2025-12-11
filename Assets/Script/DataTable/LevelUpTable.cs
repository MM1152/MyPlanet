using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class LevelUpTable : DataTable
{
    private readonly int tableId = 11000000;
    private Dictionary<(int towerId , int level), Data> levelUpTable = new Dictionary<(int towerId, int level), Data>();
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
        17  드론 갯수
        18  드론 체력
    */
    public class Data
    {
        public int ID { get; set; }
        public int Tower_ID { get; set; }
        public int Damage { get; set; }
        public int LV { get; set; }

        public int Var1 { get; set; }
        public float Val1 { get; set; }

        public int Var2 { get; set; }
        public float Val2 { get; set; }

        public int Var3 { get; set; }
        public float Val3 { get; set; }

        public int Var4 { get; set; }
        public float Val4 { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path);

        var result = await LoadCSV<Data>(textAsset.text);

        foreach(var data in result)
        {
            levelUpTable.Add((data.Tower_ID , data.LV), data);
        }

        return (filename, this);
    }

    public Data Get(int towerId , int level)
    {
        if(levelUpTable.TryGetValue((towerId , level), out var data))
        {
            return data;
        }
        return null;
    }

    public Dictionary<(int towerId, int level), Data> GetAllDataToDeepCopy()
    {
        Dictionary<(int towerId, int level), Data> copy = new Dictionary<(int towerId, int level), Data>(levelUpTable);
        return copy;
    }


#if UNITY_EDITOR
    public async UniTask<bool> SaveData(string filename, List<Data> datas)
    {
        try
        {
            var path = string.Format(FormatPath, filename);
            var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

            var assetPath = AssetDatabase.GetAssetPath(textAsset);
            var fullPath = Path.Combine(Application.dataPath, assetPath.Substring("Assets/".Length));

            using (var writer = new StreamWriter(fullPath))
            using (var csv = new CsvWriter(writer, culture: CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(datas);
            }
#if UNITY_EDITOR 
            AssetDatabase.Refresh();
#endif
            for (int i = 0; i < datas.Count; i++)
            {
                levelUpTable[(datas[i].Tower_ID , datas[i].LV)] = datas[i];
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }
#endif
}
