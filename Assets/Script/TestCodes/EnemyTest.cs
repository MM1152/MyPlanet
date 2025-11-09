using UnityEngine;

public class EnemyTest : MonoBehaviour , IDamageAble
{
    public bool IsDead => isDead;
    private bool isDead = false;
    private int hp = 10000000;
    public void OnDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Enemy Damaged {hp}");
    }

    public void OnDead()
    {
        throw new System.NotImplementedException();
    }
}
