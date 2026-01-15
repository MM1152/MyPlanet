using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections.Generic;

public enum TutorialType
{
    None = 0,
    Custom = 1,
}

public class TutorialTable : DataTable
{
    private Dictionary<TutorialStep, List<Data>> tutorialTables = new Dictionary<TutorialStep, List<Data>>();
    private Dictionary<int, Tutorial> customTutorials = new Dictionary<int, Tutorial>()
    {
        {5, new Stage1Tutorial1() },
        {6, new Stage1Tutorial2() },
    };
    public class Data
    {
        public int ID { get; set; }
        public int Order { get; set; }
        [CsvHelper.Configuration.Attributes.Name("TutorialStep")]
        public int tutorialStep { get; set; }
        public int TutorialAreaPosition { get; set; }
        [Name("ClipType")]
        public int? clipType { get; set; }
        [Name("Clip1")]
        public int? clip1 { get; set; }
        [Name("Clip2")]
        public int? clip2 { get; set; }
        [Name("Clip3")]
        public int? clip3 { get; set; }
        public string TutorialText { get; set; }
        public bool BackGroundLayoutRayCast { get; set; }
        public bool CanNextPlay { get; set; }
        [Name("TargetButtonID")]
        public int? targetButtonID { get; set; }
        [CsvHelper.Configuration.Attributes.Name("TutorialType")]
        public int tutorialType { get; set; }
        public int TimeScale { get; set; }

        [Ignore]
        public TutorialType TutorialType => (TutorialType)tutorialType;
        [Ignore]
        public TutorialStep TutorialStep => (TutorialStep)tutorialStep;
        [Ignore]
        public int Clip1 => (clip1 == null ? -1 : (int)clip1);
        [Ignore]
        public int Clip2 => (clip2 == null ? -1 : (int)clip2);
        [Ignore]
        public int Clip3 => (clip3 == null ? -1 : (int)clip3);
        [Ignore]
        public int ClipType => (clipType == null ? -1 : (int)clipType);
        [Ignore]
        public int TargetButtonID => (targetButtonID == null ? -1 : (int)targetButtonID);
    }

    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var result = await Addressables.LoadAssetAsync<TextAsset>(path).ToUniTask();
        var datas = await LoadCSV<Data>(result.text);

        foreach(var data in datas)
        {
            if(tutorialTables.ContainsKey(data.TutorialStep))
            {
                tutorialTables[data.TutorialStep].Add(data);
            }
            else
            {
                tutorialTables.Add(data.TutorialStep, new List<Data>() { data });
            }
        }

        foreach(var key in tutorialTables.Keys)
        {
            tutorialTables[key].Sort((x,y) => x.Order.CompareTo(y.Order));
        }

        return (filename, this);
    }

    public Dictionary<TutorialStep, List<Data>> GetAllTutorialData()
    {
        return tutorialTables;
    }

    public Tutorial GetCustomTutorialInstance(int tutorialId)
    {
        if(customTutorials.ContainsKey(tutorialId))
        {
            return customTutorials[tutorialId];
        }
        return null;
    }
}