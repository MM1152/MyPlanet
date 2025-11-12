using UnityEngine;
using Cysharp.Threading.Tasks;

public class Exp : MonoBehaviour
{
    [SerializeField]
    private GameObject defenseTower;
    [SerializeField]
    private GameObject towerManager;
    private bool moveing = false;
    private bool isWaiting = false;
    private float speed = 1f;
    private float distance;
    public int exp;

       


     [SerializeField]
     private string targetTag = "DefenseTower";
    private string towerTag = "TowerManager"; 

    private void OnEnable()
    {
        defenseTower = GameObject.FindGameObjectWithTag(targetTag);
        towerManager = GameObject.FindGameObjectWithTag(towerTag);        
        moveing = false;
        isWaiting = true;
        AwaitMove().Forget();
    }

    private async UniTask AwaitMove()
    {
        await UniTask.Delay(1000);
        isWaiting = false;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            towerManager.GetComponent<TowerManager>().AddExp(exp);
            Destroy(gameObject);
        }
    }

    private void FindTower()
    {
        if (isWaiting ||defenseTower == null)
        {
            return;
        }
        
        distance = Vector3.Distance(defenseTower.transform.position, transform.position);

        if (distance < 100f)
        {
            moveing = true;
        }
        else
        {            
            moveing = false;
        }
    }

    private void Update()
    {        
        FindTower();

        if (moveing)
        {         
            transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag(targetTag).transform.position, speed * Time.deltaTime);
        }
    }
}
