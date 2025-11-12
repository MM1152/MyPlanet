using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class Exp : MonoBehaviour
{
    [SerializeField]
    private GameObject defenseTower;
    [SerializeField]
    private GameObject towerManager;
    private bool isWaiting = false;
    private float speed = 1f;
    public int exp;

    private void Start()
    {
        defenseTower = GameObject.FindGameObjectWithTag(TagIds.DefenseTowerTag);
        towerManager = GameObject.FindGameObjectWithTag(TagIds.TowerManagerTag);
    }

    private void OnEnable()
    {
        isWaiting = true;
        AwaitMove().Forget();
    }

    private async UniTask AwaitMove()
    {
        await UniTask.Delay(1000);
        isWaiting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagIds.DefenseTowerTag))
        {
            towerManager.GetComponent<TowerManager>().AddExp(exp);
            ObjectPoolManager.Instance.Despawn(2, gameObject);
        }
    }

    private void FindTower()
    {
        if (isWaiting || defenseTower == null)
        {
            return;
        }
    }

    private void Update()
    {        
        FindTower();
        if (!isWaiting)
            transform.position = Vector3.MoveTowards(transform.position, defenseTower.transform.position, speed * Time.deltaTime);
    }
}
