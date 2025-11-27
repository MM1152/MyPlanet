using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class SpriteTable : DataTable
{
    private Dictionary<string , Dictionary<int , Sprite>> spriteTable = new Dictionary<string , Dictionary<int , Sprite>>();
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
                if (spriteTable.ContainsKey(filename))
                {
                    var sprite = await Addressables.LoadAssetAsync<Sprite>(data.Path);
                    spriteTable[filename].Add(data.ID, sprite);
                }
                else
                {
                    var sprite = await Addressables.LoadAssetAsync<Sprite>(data.Path);
                    spriteTable.Add(filename, new Dictionary<int, Sprite>());
                    spriteTable[filename].Add(data.ID, sprite);
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return ("SpriteTable", this);
    }

    public Sprite Get(string filename , int id)
    {
        return spriteTable[filename][id];
    }
}
