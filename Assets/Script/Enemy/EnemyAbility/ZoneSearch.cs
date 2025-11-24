using System.Collections.Generic;
using UnityEngine;

public class ZoneSearch : MonoBehaviour
{
    Enemy enemy;
    CircleCollider2D circleCollider;
    public List<Enemy> enemiesInZone = new List<Enemy>();

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Init(Enemy enemy)
    {
        if (circleCollider == null) return;

        float scale = transform.lossyScale.x;  
        circleCollider.radius = enemy.attackRange / scale;
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
            enemiesInZone.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && enemiesInZone.Contains(enemy))
        {   
            enemiesInZone.Remove(enemy);
        }
    }

    private void OnDrawGizmos()
    {
        if (circleCollider == null)
            circleCollider = GetComponent<CircleCollider2D>();

        if (circleCollider == null) return;

        float scale = transform.lossyScale.x;  // x축 기준 스케일 (2D에서 일반적으로 사용)
        float scaledRadius = circleCollider.radius * scale;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + (Vector3)circleCollider.offset, scaledRadius);
    }

}
