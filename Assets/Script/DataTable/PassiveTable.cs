using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class PassiveTable : DataTable
{
    private Dictionary<int , Data> passiveTable = new Dictionary<int, Data>();
    public class Data
    {
        public int ID { get; set; }
        public int Condition { get; set; }
        public int Effect_Id { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Val")]
        public int val { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Time")]
        public int time { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Cool_Time")]
        public int cool_Time { get; set; }
        public int Target { get; set; }
        public int Name { get; set; }
        public int Explanation { get; set; }
        
        [CsvHelper.Configuration.Attributes.Ignore]
        public int Val => DataTableManager.PlanetPassiveLevelUpTable.Get((PlanetId , FirebaseManager.Instance.PlanetData.GetOrigin(PlanetId).star)) == null ? 
            val : DataTableManager.PlanetPassiveLevelUpTable.Get((PlanetId, FirebaseManager.Instance.PlanetData.GetOrigin(PlanetId).star)).Value;
        [CsvHelper.Configuration.Attributes.Ignore]
        public int Time => DataTableManager.PlanetPassiveLevelUpTable.Get((PlanetId , FirebaseManager.Instance.PlanetData.GetOrigin(PlanetId).star)) == null ?
            time : DataTableManager.PlanetPassiveLevelUpTable.Get((PlanetId, FirebaseManager.Instance.PlanetData.GetOrigin(PlanetId).star)).Time;
        [CsvHelper.Configuration.Attributes.Ignore]
        public int Cool_Time => DataTableManager.PlanetPassiveLevelUpTable.Get((PlanetId , FirebaseManager.Instance.PlanetData.GetOrigin(PlanetId).star)) == null ?
            cool_Time : DataTableManager.PlanetPassiveLevelUpTable.Get((PlanetId, FirebaseManager.Instance.PlanetData.GetOrigin(PlanetId).star)).Cool_Time;
        [CsvHelper.Configuration.Attributes.Ignore]
        public int PlanetId => (ID % 100) + 1000;

    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var result = await LoadCSV<Data>(textAsset.text);

        for(int i = 0; i < result.Count; i++)
        {
            passiveTable.Add(result[i].ID, result[i]);
        }

        return (filename, this);
    }

    public Data GetData(int id)
    {
        if(passiveTable.TryGetValue(id, out var data))
        {
            return data;
        }
        return null;
    }
}
