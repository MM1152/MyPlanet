using UnityEngine;

public class LaserShot : IShotStrategy
{
    private bool isInitialized = false;
    float offset = 0.3f;
    LineRenderer lineRenderer;
    RaycastHit2D hit;
    LayerMask obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
    public void Shot(Enemy enemy, GameObject target)
    {
        if (target == null) return;

        if (hit.collider.gameObject.layer == target.layer)
        {
            Debug.Log("Laser Hit Player");
            var find = hit.collider.GetComponent<IDamageAble>();
            if (find != null)
            {
                float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                find.OnDamage((int)(enemy.atk * percent));
            }
        }
    }

    public void laserUpdate(Enemy enemy, GameObject target)
    {
        if (target == null) return;

        if (!isInitialized)
        {
            lineRenderer = enemy.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.startWidth = enemy.transform.localScale.y * 0.5f;
            lineRenderer.endWidth = enemy.transform.localScale.y * 0.5f;
            lineRenderer.positionCount = 2;
            isInitialized = true;
        }
        lineRenderer.SetPosition(0, enemy.transform.position);

        Vector2 dir = (target.transform.position - enemy.transform.position).normalized;
        float dis = Vector2.Distance(enemy.transform.position, target.transform.position);

        hit = Physics2D.Raycast(enemy.transform.position, dir, dis, obstacleMask);
        if (hit.collider != null)
        {
            Vector2 offsetPoint = hit.point + dir * offset;
            lineRenderer.SetPosition(1, offsetPoint);
        }
    }
}

