using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Data.Common;

public class SoundTable : DataTable
{
    private Dictionary<int, List<AudioClip>> soundTable = new Dictionary<int, List<AudioClip>>();
    public class Data
    {
        public int ID { get; set; }
        public string Path { get; set; }
    }
    public override async UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = await Addressables.LoadAssetAsync<TextAsset>(path);
        var result = await LoadCSV<Data>(textAssets.text);
        try
        {
            foreach (var data in result)
            {
                var clip = await Addressables.LoadAssetAsync<AudioClip>(data.Path);

                if (!soundTable.ContainsKey(data.ID))
                {
                    soundTable.Add(data.ID, new List<AudioClip>() { clip });
                }
                else
                {
                    soundTable[data.ID].Add(clip);
                }
            }
        }
        catch (System.Exception ex)
        {
        }
        return (filename, this);
    }
    public AudioClip Get(int type , int index)
    {
        if (soundTable.ContainsKey(type) && index != -1)
        {
            return soundTable[type][index];
        }
        return null;
    }
}