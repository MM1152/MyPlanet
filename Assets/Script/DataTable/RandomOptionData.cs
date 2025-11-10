using System.Collections.Generic;

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
        optionTable.Add(1 , new Data() {id = 1, description = "양옆 타워 공격력 증가"  , option = new ToweredamageUpgradOption()});
        optionTable.Add(2 , new Data() {id = 2, description = "전체 타워 공격력 증가"  , option = new ToweredamageUpgradOption()});
        optionTable.Add(3 , new Data() {id = 3, description = "행성 회복"  , option = new RepairBasePlanetOption()});
    }

    public Data GetData(int id)
    {
        return optionTable[id];
    }
}