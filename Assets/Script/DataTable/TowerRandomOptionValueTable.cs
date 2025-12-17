using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

public class TowerRandomOptionValueTable : DataTable
{
    private Dictionary<int, Data> towerRandomoptionTable = new Dictionary<int, Data>();

    public class Data
    {
        public int randmom_ID { get; set; }
        public int tower_ID { get; set; }
        public int random_min { get; set; }
        public int tower_grade_ID_1 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("1_random_max")]
        public int random_max_1 { get; set; }
        public int tower_grade_ID_2 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("2_random_max")]
        public int random_max_2 { get; set; }
        public int tower_grade_ID_3 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("3_random_max")]
        public int random_max_3 { get; set; }
        public int tower_grade_ID_4 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("4_random_max")]
        public int random_max_4 { get; set; }
        public int tower_grade_ID_5 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("5_random_max")]
        public int random_max_5 { get; set; }

        public int GetGradeToId(int grade)
        {
            return grade switch
            {
                1 => tower_grade_ID_1,
                2 => tower_grade_ID_2,
                3 => tower_grade_ID_3,
                4 => tower_grade_ID_4,
                5 => tower_grade_ID_5,
                _ => 0
            };
        }
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var result = await LoadCSV<Data>(textAsset.text);

        foreach(var data in result)
        {
            towerRandomoptionTable.Add(data.tower_ID, data);
        }

        return (filename, this);
    }

    public Data GetOptionData(int towerId)
    {
        return towerRandomoptionTable[towerId];
    }

    public (float percent , int LMH) GetRandomOptionValue(int towerId , int grade)
    {
        var data = GetOptionData(towerId);

        var gradeId = 25000 + grade + 100 * grade;
        var randomValueMax = gradeId switch
        {
            int id when id == data.tower_grade_ID_1 => data.random_max_1 ,
            int id when id == data.tower_grade_ID_2 => data.random_max_2 ,
            int id when id == data.tower_grade_ID_3 => data.random_max_3,
            int id when id == data.tower_grade_ID_4 => data.random_max_4,
            int id when id == data.tower_grade_ID_5 => data.random_max_5,
            _ => 0
        };

        return DataTableManager.TowerRandomOptionValuePercentTable.GetRandomOptionValuePercent(gradeId , data.random_min , randomValueMax);
    }

    public bool IsMaxGrade(int towerId, int grade , float currentValue)
    {
        var data = GetOptionData(towerId);

        var gradeId = 25000 + grade + 100 * grade;
        Debug.Log($"Tower Max Value : {GetMaxPercent(towerId, grade)} , Current Valuue : {currentValue}");
        return gradeId switch
        {
            int id when id == data.tower_grade_ID_1 => data.random_max_1 == currentValue,
            int id when id == data.tower_grade_ID_2 => data.random_max_2 == currentValue,
            int id when id == data.tower_grade_ID_3 => data.random_max_3 == currentValue,
            int id when id == data.tower_grade_ID_4 => data.random_max_4 == currentValue,
            int id when id == data.tower_grade_ID_5 => data.random_max_5 == currentValue,
            _ => false
        };
    }

    public int GetMaxPercent(int towerId, int grade)
    {
        var data = GetOptionData(towerId);

        var gradeId = 25000 + grade + grade * 100;
        return gradeId switch
        {
            int id when id == data.tower_grade_ID_1 => data.random_max_1,
            int id when id == data.tower_grade_ID_2 => data.random_max_2,
            int id when id == data.tower_grade_ID_3 => data.random_max_3,
            int id when id == data.tower_grade_ID_4 => data.random_max_4,
            int id when id == data.tower_grade_ID_5 => data.random_max_5,
            _ => -1
        };
    }
}
