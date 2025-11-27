using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class RotatingLaserAttack : IShotStrategy
{
    private Rect screenBounds;

    private LineRenderer laserRenderer;

    private RaycastHit2D hit;

    Vector3 targetPos = Vector3.zero;
    private LayerMask obstacleMask = LayerMask.GetMask("DefenseTower", "Player");

    private bool isInitialized = false;

    Vector2 endPoint = Vector2.zero;

    public float rotationInterval = 10f;

    private float rotationSpeed = 1f;
    private float rotationAngle = 90f;

    private float currentAngle = 0f;
    private float maxdistance = 0f;

    private float startAngle = 0f;

    private float endAngle = 180f;

    private float delayTime = 1f;

    private float delayTimer = 0f;

    private float damageTimer = 0f;

    public void Shot(Enemy enemy, GameObject target)
    {
        damageTimer += Time.deltaTime;
        if (damageTimer > enemy.fireInterval)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == target.layer)
                {
                    var find = hit.collider.GetComponent<IDamageAble>();
                    if (find != null)
                    {
                        float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                        find.OnDamage((int)(enemy.atk * percent));
                    }
                }
            }
            damageTimer = 0f;
        }

    }

    public void UpdateLaser(Enemy enemy, GameObject target)
    {
        if (target == null || target.transform == null)
        {
            LaserReset(enemy);
            return;
        }

        if (!isInitialized)
        {
            screenBounds = enemy.WaveManager.ScreenBounds;
            laserRenderer = enemy.enemyLineRenderer;
            laserRenderer.enabled = true;
            laserRenderer.startWidth = enemy.transform.localScale.y * 0.3f;
            laserRenderer.endWidth = enemy.transform.localScale.y * 0.3f;
            laserRenderer.positionCount = 2;
            laserRenderer.material.color = Color.yellow;
            isInitialized = true;
            currentAngle = 0f;
            maxdistance = Mathf.Max(screenBounds.width, screenBounds.height) * 1.5f;
            startAngle = 0f;
            delayTimer = 0f;
        }

        if (delayTimer < delayTime)
        {
            delayTimer += Time.deltaTime;

            Vector2 dirFixed = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
            laserRenderer.SetPosition(0, enemy.transform.position);

            RaycastHit2D hitTemp = Physics2D.Raycast(enemy.transform.position, dirFixed, maxdistance, obstacleMask);
            if (hitTemp.collider != null)
            {
                laserRenderer.SetPosition(1, hitTemp.point + dirFixed * 0.1f);
            }
            else
            {
                laserRenderer.SetPosition(1, (Vector2)enemy.transform.position + dirFixed * maxdistance);
            }
            return;
        }

        currentAngle += rotationAngle * rotationSpeed * Time.deltaTime;
        startAngle += rotationAngle * rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
        }

        Vector2 dir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

        laserRenderer.SetPosition(0, enemy.transform.position);

        hit = Physics2D.Raycast(enemy.transform.position, dir, maxdistance, obstacleMask);

        if (hit.collider != null)
        {
            Vector2 offsetPoint = hit.point + dir * 0.1f;
            laserRenderer.SetPosition(1, offsetPoint);
        }
        else
        {
            endPoint = (Vector2)enemy.transform.position + dir * maxdistance;
            laserRenderer.SetPosition(1, endPoint);
        }

        if (endAngle - startAngle <= 0f)
        {
            LaserReset(enemy);
        }
    }


    public void LaserReset(Enemy enemy)
    {
        if (laserRenderer != null)
        {
            laserRenderer.enabled = false;
            laserRenderer.positionCount = 0; ;
            enemy.attackInterval = 0f;
        }
        isInitialized = false;
    }
}
