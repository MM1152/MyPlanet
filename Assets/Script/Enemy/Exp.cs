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
    [SerializeField]
    private float speed = 1f;
    private int exp;
    [SerializeField] Sprite oneExpSprite;
    [SerializeField] Sprite twoExpSprite;
    [SerializeField] Sprite threeExpSprite;
    [SerializeField] SpriteRenderer spriteRenderer;  

    private void Awake()
    {
        speed = DataTableManager.OptionTable.GetValueDataToInt(5013);
    } 

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

    public void Init(int exp)
    {
        this.exp = exp;
        SetSprite(exp);
    }

    private void SetSprite(int exp)
    {
        switch (exp)
        {
            case 1:
                spriteRenderer.sprite = oneExpSprite;
                break;
            case 2:
                spriteRenderer.sprite = twoExpSprite;
                break;
            case 3:
                spriteRenderer.sprite = threeExpSprite;
                break;
            default:
                break;
        }
    }

    private async UniTask AwaitMove()
    {
        await UniTask.Delay(1000 , cancellationToken : this.gameObject.GetCancellationTokenOnDestroy());
        isWaiting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagIds.DefenseTowerTag))
        {
            towerManager.GetComponent<TowerManager>().AddExp(exp);
            Managers.ObjectPoolManager.Despawn(PoolsId.Exp, gameObject);
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
