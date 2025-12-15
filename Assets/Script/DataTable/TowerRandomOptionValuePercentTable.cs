using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
public class TowerRandomOptionValuePercentTable : DataTable
{
    private Dictionary<int, Data> towerRandomoptionPercentTable = new Dictionary<int, Data>();

    public class Data
    {
        public int tower_grade_ID { get; set; }
        public int tower_grade { get; set; }
        public int step_count { get; set; }
        public int low_range { get; set; }
        public int mid_range { get; set; }
        public int high_range { get; set; }
        public float low_prob { get; set; }
        public float mid_prob { get; set; }
        public float high_prob { get; set; }
    }
    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var result = await LoadCSV<Data>(textAsset.text);
        foreach(var data in result)
        {
            towerRandomoptionPercentTable.Add(data.tower_grade_ID, data);
        }
        return (filename, this);
    }

    public Data GetData(int grade_Id)
    {
        return towerRandomoptionPercentTable[grade_Id];
    }

    public (float percent , int LMH) GetRandomOptionValuePercent(int grade_Id , float minValue , float maxValue)
    {
        var data = GetData(grade_Id);

        float rand = Random.Range(0f, 1f);
        float sliceValue = (maxValue - minValue) / data.step_count;

        List<float> valueList = new List<float>();
        for(int i = 0; i < data.step_count + 1; i++)
        {
            valueList.Add(minValue + sliceValue * i);
        }

        if (rand <= data.low_prob)
        {
            int randomIndex = Random.Range(0, data.low_range + 1);
            Debug.Log($"<color=green>Random => 하 등급 뽑음 {valueList[randomIndex]}% 적용</color>");
            return (valueList[randomIndex] , 0);
        }
        else if (rand <= data.low_prob + data.mid_prob)
        {
            int randomIndex = Random.Range(data.low_range + 1, data.mid_range + 1);
            Debug.Log($"<color=yellow>Random => 중 등급 뽑음 {valueList[randomIndex]}% 적용</color>");
            return (valueList[randomIndex] , 1);
        }
        else
        {
            int randomIndex = Random.Range(data.mid_range + 1, data.high_range + 1);
            Debug.Log(@$"<color=red>Random => 상 등급 뽑음 {valueList[randomIndex]}% 적용</color>");
            if(randomIndex == valueList.Count - 1)
            {
                Debug.Log("<color=red>최고 등급이 뽑혔습니다!</color>");
            }
            return (valueList[randomIndex] , 2);
        }
    }
}