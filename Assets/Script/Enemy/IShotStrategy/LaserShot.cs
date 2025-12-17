using UnityEngine;

public class LaserShot : IShotStrategy
{
    private bool isInitialized = false;
    private float offset = 0.1f;
    private LineRenderer lineRenderer;
    private RaycastHit2D hit;
    private LayerMask obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
    public void Shot(Enemy enemy, GameObject target)
    {
        if (target == null || hit.collider == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        if (hit.collider.gameObject.layer == target.layer)
        {
            var find = hit.collider.GetComponent<IDamageAble>();
            if (find != null)
            {
                float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                find.OnDamage(Mathf.Clamp((int)((enemy.atk-find.Defense)* percent), 1, int.MaxValue));    
            }
        }
    }

    public void LaserUpdate(Enemy enemy, GameObject target)
    {
        if (target == null || target.transform == null) return;

        if (!isInitialized)
        {
            lineRenderer = enemy.enemyLineRenderer;
            lineRenderer.enabled = true;
            lineRenderer.startWidth = enemy.transform.localScale.y * 0.3f;
            lineRenderer.endWidth = enemy.transform.localScale.y * 0.3f;
            lineRenderer.positionCount = 2;
            isInitialized = true;
        }
        lineRenderer.SetPosition(0, enemy.transform.position);

        Vector2 dir = (target.transform.position - enemy.transform.position).normalized;
        float dis = Vector2.Distance(enemy.transform.position, target.transform.position);

        hit = Physics2D.Raycast(enemy.transform.position, dir, dis);
        if (hit.collider != null)
        {
            if(hit.collider.CompareTag(target.tag) || hit.collider.CompareTag(TagIds.PlayerTag) 
                || hit.collider.CompareTag(TagIds.DefenseTowerTag))
            {
                lineRenderer.SetPosition(1, hit.collider.transform.position);
            }
        }
    }

    public void LaserReset()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
        isInitialized = false;
    }
}

