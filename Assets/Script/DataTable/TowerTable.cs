using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TowerTable : DataTable
{
    private Dictionary<int, Data> towerTable = new Dictionary<int, Data>();
    private Dictionary<int, Tower> towerKind = new Dictionary<int, Tower>()
    {
        { 1 , new HellFireGunTower() },
        { 2 , new VolcanoLauncher() },
        { 3 , new LaserTower() }
    };
    private Dictionary<int, string> towerAttackPrefabs = new Dictionary<int, string>()
    {
        { 1 , "Bullet" },
        { 2 , "Missile" },
        { 3 , "Laser" }
    };
    public class Data
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public int Explanation { get; set; }
        public char Grade { get; set; }
        public int Attribute { get; set; }
        public int ATK { get; set; }
        public float AttackRadius { get; set; }
        public string Bullet_Path { get; set; }
        public string Image_Path { get; set; }
        public int Fire_Rate { get; set; }
        public int Option { get; set; }
        public int Min_Value { get; set; }
        public int Max_Value { get; set; }
        public int Option_Type { get; set; }
        public int Option_Range { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public Tower tower;
        [CsvHelper.Configuration.Attributes.Ignore]
        public string projectilePrefabPath;
    }

    //리소스로딩이아닌 어드레서블로 로딩으로 변경

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var ReAssets = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSVTest<Data>(ReAssets.text);

        foreach (var data in datas)
        {
            data.tower = towerKind[data.ID];
            data.projectilePrefabPath = towerAttackPrefabs[data.ID];
            towerTable.Add(data.ID, data);
        }

        return (filename, this as DataTable);
    }

    public Data Get(int id)
    {
        if (!towerTable.ContainsKey(id))
        {
#if DEBUG_MODE
            throw new System.Exception($"TowerData 에는 ID {id} 가 존재하지 않습니다.");
#endif
            return null;
        }

        return towerTable[id];
    }
}
