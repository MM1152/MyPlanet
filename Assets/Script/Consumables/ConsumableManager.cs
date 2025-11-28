using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    [Header("Referense")]
    [SerializeField] private TowerManager towerManger;
    [SerializeField] private BasePlanet planet;

    private void Awake()
    {
        Test().Forget();
        Test().Forget();
        Test().Forget();
    }

    private async UniTaskVoid Test()
    {
        Debug.Log("1");
        await UniTask.Delay(1000);
        Debug.Log("2");
        await UniTask.Delay(1000);
        Debug.Log("3");
        await UniTask.Delay(1000);
        Debug.Log("4");
        await UniTask.Delay(1000);
        Debug.Log("5");
    }
}
