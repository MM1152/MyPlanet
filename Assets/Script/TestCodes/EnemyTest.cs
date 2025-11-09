using UnityEngine;

public class EnemyTest : MonoBehaviour , IDamageAble
{
    public ElementType enemyType;
    public bool IsDead => isDead;
    public ElementType ElementType => enemyType;
    private bool isDead = false;
    private int hp = 10000000;
    public void OnDamage(int damage)
    {
        hp -= damage;
    }

    public void OnDead()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var find = collision.GetComponent<IDamageAble>();
            if(find != null)
            {
                find.OnDamage(5);
            }
        }
    }
}
