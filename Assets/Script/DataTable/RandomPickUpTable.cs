using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class RandomPickUpTable : DataTable
{
    private Dictionary<int , List<Data>> randomPickUpTable = new Dictionary<int, List<Data>>();

    private List<Data> sortedPlanetRandomDatas;
    private List<Data> sortedTowerRandomDatas;

    private float towerWeight = 0f;
    private float planetWeight = 0f;
    public class Data
    {
        public int reward_id { get; set; }
        public int? reward_name_id { get; set; }
        public int G_item_id { get; set; }
        public int item_type { get; set; }
        public int? grade { get; set; }
        public int reward_type { get; set; }
        public int? amount { get; set; }
        public int? tower_random_id { get; set; }
        public float probability { get; set; }
        public int connection_id { get; set; }
        [CsvHelper.Configuration.Attributes.Ignore]
        public string RewardName
        {
            get
            {
                if(reward_type == 1)
                    return DataTableManager.PlanetTable.Get(connection_id).Name;
                else if (reward_type == 3)
                    return DataTableManager.TowerTable.Get(connection_id).Name;
                
                return $"{DataTableManager.StringTable.Get(reward_name_id ?? 0)} {amount}개";
            }
        }
        [CsvHelper.Configuration.Attributes.Ignore]
        public bool IsPlanetReward => reward_id == 171001;
        [CsvHelper.Configuration.Attributes.Ignore]
        public bool IsTowerReward => reward_id == 172002;
        [CsvHelper.Configuration.Attributes.Ignore]
        public string rarityToString => grade switch
        {
            1 => "S",
            2 => "A",
            3 => "B",
            4 => "C",
            _ => "정의되지 않음"
        };
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path);
        var results = await LoadCSV<Data>(textAssets.text);

        foreach (var data in results)
        {
            //FIX : 임시로 막아놓음
            if (data.connection_id == 23008) continue;
            if(randomPickUpTable.ContainsKey(data.reward_id))
            {
                randomPickUpTable[data.reward_id].Add(data);

                if (data.IsPlanetReward)
                    planetWeight += data.probability;
                else
                    towerWeight += data.probability;
            }
            else
            {
                randomPickUpTable.Add(data.reward_id, new List<Data>());
                randomPickUpTable[data.reward_id].Add(data);

                if (data.IsPlanetReward)
                    planetWeight += data.probability;
                else   
                    towerWeight += data.probability;
            }
        }

        sortedPlanetRandomDatas = new List<Data>(randomPickUpTable[171001]);
        sortedPlanetRandomDatas.Sort((a, b) => a.probability.CompareTo(b.probability));
        sortedTowerRandomDatas = new List<Data>(randomPickUpTable[172002]);
        sortedTowerRandomDatas.Sort((a, b) => a.probability.CompareTo(b.probability));

        return (filename, this);
    }

    public Data GetRandomDataForPlanet()
    {
        float rand = Random.Range(0f, planetWeight);
        float probabilityAmount = 0f;
        foreach (var data in sortedPlanetRandomDatas)
        {
            probabilityAmount += data.probability;
            if (rand <= probabilityAmount)
            {
                return data;
            }
        }

        return null;
    }

    public Data GetRandomDataForTower()
    {
        float rand = Random.Range(0f, towerWeight);
        float probabilityAmount = 0f;
        foreach (var data in sortedTowerRandomDatas)
        {
            //FIX : 임시로 막아놓음
            probabilityAmount += data.probability;
            if (rand <= probabilityAmount)
            {
                return data;
            }
        }

        return null;
    }

    public List<Data> GetAllDataForPlanet()
    {
        return randomPickUpTable[171001];
    }

    public List<Data> GetRandomPickUpDatasForTower()
    {
        return randomPickUpTable[172002];
    }
}