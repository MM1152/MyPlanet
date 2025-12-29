using UnityEngine;

public abstract class EnemyAssetMove : MonoBehaviour
{
    [SerializeField] protected Transform enemyAsset;
    
    protected virtual float speed { get;}
    public abstract void Move();

    public void Update()
    {
        Move();
    }
}
