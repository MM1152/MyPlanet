using System.Collections.Generic;
using UnityEngine;
public class RandomOptionData
{
    public class Data
    {
        public int id;
        public string description;

        [CsvHelper.Configuration.Attributes.Ignore]
        public RandomOptionBase option;
    }
    public Dictionary<int, Data> optionTable = new Dictionary<int, Data>();

    public RandomOptionData()
    {
        optionTable.Add(1 , new Data() {id = 1, description = "양옆 {0}칸 타워의 공격력 {1}% 만큼 증가"  , option = new TowerDamageUpgradeInRange()});
        optionTable.Add(2 , new Data() {id = 2, description = "전체 타워 공격력 {0}% 만큼 증가"  , option = new AllTowerDamageUpgradeOption()});
        optionTable.Add(3 , new Data() {id = 3, description = "행성 최대체력의 {0}% 만큼 3초마다 회복"  , option = new RepairBasePlanetOption()});
    }

    public Data GetData(int id)
    {
        return optionTable[id];
    }

    public Data GetRandomOption()
    {
        int rand = Random.Range(1 , optionTable.Count + 1);
        Debug.Log($"RandomOption : {rand}");
        return optionTable[rand];
    }

    public RandomOptionBase GetRandomOptionBase(int id)
    {
        return optionTable[id].option.DeepCopy();
    }
}