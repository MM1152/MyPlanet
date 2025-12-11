using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ZoneSearch : MonoBehaviour
{
    private Enemy enemy;
    private CircleCollider2D circleCollider;
    public List<Enemy> enemiesInZone = new List<Enemy>();

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Init(Enemy enemy)
    {
        if (circleCollider == null) return;

        this.enemy = enemy;
        float scale = transform.lossyScale.x;
        circleCollider.radius = SetScaledRadius();
    }

    private float SetScaledRadius()
    {
        return enemy.ElementType switch
        {
            ElementType.Fire => DataTableManager.OptionTable.GetValueDataToFloat(5036),
            ElementType.Ice => DataTableManager.OptionTable.GetValueDataToFloat(5036),
            ElementType.Steel => DataTableManager.OptionTable.GetValueDataToFloat(5036),
            ElementType.Light => DataTableManager.OptionTable.GetValueDataToFloat(5036),
            ElementType.Dark => DataTableManager.OptionTable.GetValueDataToFloat(5036),
            _ => 0f,
        };
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


        if (collision.CompareTag(TagIds.DroneTag))
        {
            if(this.enemy.enemyType == EnemyType.Ranged)
            {
                var drone = collision.GetComponent<Drone>();
                var percent = drone.Tower.BonusDroneTargetedPercent / 100f;
                var rand = UnityEngine.Random.Range(0f, 1f);

                if (rand < percent)
                {
                    this.enemy.SetTarget(collision.gameObject);
                    this.enemy.SetState(this.enemy.stateMachine.attackState);
                    return;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && enemiesInZone.Contains(enemy))
        {
            enemiesInZone.Remove(enemy);
        }
        if (collision.CompareTag(TagIds.DroneTag))
        {
            if (this.enemy.GetTarget() == collision.gameObject)
            {
                this.enemy.SetTarget(GameObject.FindGameObjectWithTag("Player").gameObject);
                this.enemy.SetState(this.enemy.stateMachine.attackState);
            }
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
