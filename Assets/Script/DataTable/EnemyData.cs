using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnemyData : DataTable
{
    private Dictionary<int, Data> enemyTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        //적의속성이저장되어있는열입니다
        public int Attribute { get; set; } = -1;    
        public int Speed { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int AttackInterval { get; set; }
        public float AttackRange { get; set; }
        public int EXP { get; set; }
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
}
