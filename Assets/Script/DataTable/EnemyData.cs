using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnemyData : DataTable
{
    private Dictionary<int, Data> enemyTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }        
        public int Attribute { get; set; } = -1;                   
        public int Speed { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int EXP { get; set; }
        public int Fire_Rate { get; set; }
        public float Range { get; set; }
        public int Bullet_Speed { get; set; }
        public string Image_Path { get; set; }
        public string Bullet_Path { get; set; }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();

        var datas = await LoadCSV<Data>(textAsset.text);

        foreach (var data in datas)
        {
            enemyTable.Add(data.ID, data);
        }

        return (filename, this as DataTable);
    }
    public Data GetData(int id)
    {
        if (!enemyTable.ContainsKey(id))
        {
#if DEBUG_MODE
            throw new System.Exception($"EnemyData  ID: {id} is not found.");
#endif
        }
        return enemyTable[id];
    }

    public List<Data> GetAllDataDeepCopy()
    {
        List<Data> deepCopyList = new List<Data>(enemyTable.Values.ToList());
        return deepCopyList;
    }
#if UNITY_EDITOR
    public async UniTask<bool> SaveData(string filename , List<Data> datas)
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
                enemyTable[datas[i].ID] = datas[i];
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
