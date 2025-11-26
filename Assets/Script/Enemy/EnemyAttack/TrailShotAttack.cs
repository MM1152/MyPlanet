using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class TrailShotAttack : IShotStrategy
{
    private bool isInitialized = false;
    private float offset = 0.1f;
    private LineRenderer lineRenderer;
    private RaycastHit2D hit;
    private LayerMask obstacleMask = LayerMask.GetMask("DefenseTower", "Player");

    private float startWidth = 0f;
    private float endWidth;
    private float lineWidth;

    private float growTime;

    public void Shot(Enemy enemy, GameObject target)
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            isInitialized = false;
        }
        Vector2 dir = (target.transform.position - enemy.transform.position).normalized;
        float offset = 0.5f;
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.transform.position = enemy.transform.position + (Vector3)(dir * offset);
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);
    }

    public void ShotLineDraw(Enemy enemy, GameObject target)
    {
        if (target == null || target.transform == null) return;

        if (!isInitialized)
        {
            lineRenderer = enemy.enemyLineRenderer;
            lineRenderer.enabled = true;
            startWidth = 0f;
            endWidth = enemy.transform.localScale.y * 0.4f;
            // lineRenderer.material.SetColor("_Color", Color.red);
            lineRenderer.material.color = Color.red;
            lineRenderer.positionCount = 2;
            isInitialized = true;
            growTime = 0f;
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
        growTime += Time.deltaTime;
        float t = growTime / 0.3f;
        lineWidth = Mathf.Lerp(startWidth, endWidth, t);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    private EnemyProjectileBase CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<EnemyProjectileBase>(poolsId);
        EnemyProjectileBase projectile = projectileObj.GetComponent<EnemyProjectileBase>();
        return projectile;
    }
}
