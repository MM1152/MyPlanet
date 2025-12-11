using System.Collections.Generic;
using UnityEngine;

public class RandomLaserAttack : IShotStrategy
{
    private LineRenderer laserRenderer;
    private RaycastHit2D hit;
    private LayerMask obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
    private bool isInitialized = false;
    private bool isTargetHit = false;
    private List<Vector2> laserPoints = new List<Vector2>();
    private Vector2 currentStartPoint;
    private int durationCount = 0;
    private int durationMax = 3;

    private void InitializeLaserRenderer(Enemy enemy)
    {
        if (isInitialized == false)
        {
            laserRenderer = enemy.enemyLineRenderer;
            laserRenderer.enabled = true;
            laserRenderer.startWidth = enemy.transform.localScale.y * 0.3f;
            laserRenderer.endWidth = enemy.transform.localScale.y * 0.3f;
            laserRenderer.positionCount = 2;
            isInitialized = true;
            laserPoints.Clear();
            laserPoints.Add(enemy.transform.position - Vector3.right*0.5f); // Left 
            laserPoints.Add(enemy.transform.position); // Mid
            laserPoints.Add(enemy.transform.position + Vector3.right*0.5f); // Right
            ResetLaserPoint();
            Debug.Log("Initialized Laser Renderer");
        }
    }

    private void ResetLaserPoint()
    {
        currentStartPoint = laserPoints[Random.Range(0, laserPoints.Count)];
    }

    public void LaserUpdate(Enemy enemy, GameObject target)
    {
        if (laserRenderer == null) InitializeLaserRenderer(enemy);

        if (target == null)
        {
            if (laserRenderer != null) laserRenderer.enabled = false;
            return;
        }

        if (durationCount >= durationMax)
        {
            durationCount = 0;    
            ResetLaserPoint();
            Debug.Log("Reset Laser Point");
        }

        laserRenderer.SetPosition(0, currentStartPoint);
        Vector2 dir = (target.transform.position - (Vector3)currentStartPoint).normalized;
        float dis = Vector2.Distance(currentStartPoint, target.transform.position);
        hit = Physics2D.Raycast(currentStartPoint, dir, dis, obstacleMask);
        if (hit.collider != null)
        {
            Vector2 offsetPoint = hit.point;
            laserRenderer.SetPosition(1, offsetPoint);
        }
    }

    public void Shot(Enemy enemy, GameObject target)
    {
        if (target == null || hit.collider == null)
        {
            if (laserRenderer != null) laserRenderer.enabled = false;
            return;
        }

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == target.layer)
            {
                Debug.Log("Laser Hit Target");  
                var find = hit.collider.GetComponent<IDamageAble>();
                if (find != null)
                {
                    float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                    var damage = Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue);
                    find.OnDamage(damage);
                    enemy.OnHeal(damage/2);
                }
            }
        }
        durationCount++;
    }

    public void LaserReset()
    {
        if (laserRenderer != null)
        {
            laserRenderer.enabled = false;
            laserRenderer.positionCount = 0;
        }
        isInitialized = false;
    }
}
