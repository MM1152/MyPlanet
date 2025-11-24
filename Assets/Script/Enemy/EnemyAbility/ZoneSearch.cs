using System.Collections.Generic;
using UnityEngine;

public class ZoneSearch : MonoBehaviour
{
    Enemy enemy;
    CircleCollider2D collider;
    public List<Enemy> enemiesInZone = new List<Enemy>();

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        collider = GetComponent<CircleCollider2D>();
    }     

    public void Init(Enemy enemy)
    {
        if(collider == null) return;
        collider.radius = enemy.attackrange;
    }

    private void OnEnable()
    {
        enemiesInZone.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemiesInZone.Contains(enemy))
        {
            Debug.Log($"ZoneSearch: Enemy {enemy.name} entered the zone.");
            enemiesInZone.Add(enemy);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && enemiesInZone.Contains(enemy))
        {
            Debug.Log($"ZoneSearch: Enemy {enemy.name} exited the zone.");
            enemiesInZone.Remove(enemy);
        }
    }
}
