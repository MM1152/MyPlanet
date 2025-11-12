using UnityEngine;
using Cysharp.Threading.Tasks;

public class Exp : MonoBehaviour
{
      [SerializeField]
    private GameObject defenseTower;
    private float speed = 1f;

    private bool moveing = false;
    private bool isWaiting = false;

     private float distance;


     [SerializeField]
     private string targetTag = "DefenseTower";

    private void OnEnable()
    {
        defenseTower = GameObject.FindGameObjectWithTag(targetTag);
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
            //경험치 획득 로직 추가
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
