using System.Collections.Generic;
using UnityEngine;
public class RandomOptionData
{
    public class Data
    {
        public int id;

        [CsvHelper.Configuration.Attributes.Ignore]
        public RandomOptionBase option;
    }
    public Dictionary<int, Data> optionTable = new Dictionary<int, Data>();

    public RandomOptionData()
    {
        optionTable.Add(1 , new Data() {id = 1, option = new TowerDamageUpgradeOption()});
        optionTable.Add(2 , new Data() {id = 2, option = new TowerAttackSpeedOption()});
        optionTable.Add(3 , new Data() {id = 3, option = new FireElemetDamageUpgradeOption()});
        optionTable.Add(4 , new Data() {id = 4, option = new IceElemetDamageUpgradeOption()});
        optionTable.Add(5 , new Data() {id = 5, option = new SteelElemetDamageUpgradeOption()});
        optionTable.Add(6 , new Data() {id = 6, option = new LightElemetDamageUpgradeOption()});
        optionTable.Add(7 , new Data() {id = 7, option = new DarkElemetDamageUpgradeOption()});
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