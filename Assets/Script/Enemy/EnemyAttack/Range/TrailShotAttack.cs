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
    private LayerMask obstacleMask;

    private float startWidth = 0f;
    private float endWidth;
    private float lineWidth;
    private float growTime;
    private Enemy enemy;
    private Vector2 startPoint;


    public void Shot(Enemy enemy, GameObject target)
    {
        this.enemy = enemy;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            isInitialized = false;
        }
        Vector2 dir = (target.transform.position - enemy.transform.position).normalized;
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);     
        Bullet.Init(enemy, enemy.typeEffectiveness);
        Bullet.SetTarget(target.transform);
    }

    public void ShotLineDraw(Enemy enemy, GameObject target)
    {
        if (target == null || target.transform == null) return;
        
        if(obstacleMask == 0)
        {
            obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
        }

        if (!isInitialized)
        {
            lineRenderer = enemy.enemyLineRenderer;
            lineRenderer.enabled = true;
            startWidth = 0f;
            endWidth = enemy.transform.localScale.y * 0.4f;            
            lineRenderer.positionCount = 2;
            isInitialized = true;
            growTime = 0f;
        }

        lineRenderer.SetPosition(0, enemy.transform.position);

        Vector2 dir = (target.transform.position - enemy.transform.position).normalized;
        float dis = Vector2.Distance(enemy.transform.position, target.transform.position);
         startPoint = enemy.transform.position + (Vector3)dir * (enemy.transform.localScale.x * 0.5f);
        lineRenderer.SetPosition(0, startPoint);

        hit = Physics2D.Raycast(enemy.transform.position, dir, dis, obstacleMask);
        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        float lineDrawDuration = enemy.fireInterval * 0.4f;
        growTime += Time.deltaTime;
        float t = growTime / lineDrawDuration;
        lineWidth = Mathf.Lerp(endWidth, startWidth, t);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    private SimpleBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<SimpleBullet>(poolsId);
        SimpleBullet projectile = projectileObj.GetComponent<SimpleBullet>();
        if (enemy.target != null)
        {
            var dir = enemy.target.transform.position - enemy.transform.position;

            projectile.SetHitParticle(PoolsId.Hit13redlaser);
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
            flash.transform.position = enemy.transform.position + dir.normalized * (enemy.transform.localScale.x * 0.5f);
            projectile.transform.position = flash.transform.position;
        }
        return projectile;
    }
}
