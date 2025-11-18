using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class TextSpawnManager : MonoBehaviour
{
    private ObjectPoolManager poolManager;

    private void Awake()
    {
        poolManager = Managers.ObjectPoolManager;
    }

    public TextUI SpawnTextUI(string content, Vector2 position)
    {
        var textUI = poolManager.SpawnObject<TextUI>(PoolsId.TextUI);
        textUI.Init(content, position);
        return textUI;
    }   
}
