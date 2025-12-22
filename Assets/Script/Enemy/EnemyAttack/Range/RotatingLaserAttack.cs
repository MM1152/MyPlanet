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
    private LayerMask obstacleMask;

    private bool isInitialized = false;

    Vector2 endPoint = Vector2.zero;

    public float rotationInterval = 10f;

    private float rotationSpeed = 0.5f;
    private float rotationAngle = 90f;

    private float currentAngle = 0f;
    private float maxdistance = 0f;

    private float startAngle = 0f;

    private float endAngle = 180f;

    private float delayTime = 1f;

    private float delayTimer = 0f;

    private float damageTimer = 0.1f;
    private Vector2 startPoint = Vector2.zero;

    private ParticleSystem hitParticle;
    private ParticleSystem flashParticle;

    public void Shot(Enemy enemy, GameObject target)
    {
        if (hit.collider != null)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > enemy.fireInterval)
            {
                if (hit.collider.gameObject.layer == target.layer)
                {
                    var find = hit.collider.GetComponent<IDamageAble>();
                    if (find != null)
                    {
                        float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
                        find.OnDamage(Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue));
                    }
                }
                damageTimer = 0f;
            }
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
            screenBounds = Utils.GetScreenBounds();
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
            damageTimer = enemy.fireInterval;
            obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
            enemy.OnDie += LaserReset;
        }

        if (delayTimer < delayTime)
        {
            delayTimer += Time.deltaTime;

            Vector2 dirFixed = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
            startPoint = enemy.transform.position + (Vector3)dirFixed * (enemy.transform.localScale.x * 0.5f);
            laserRenderer.SetPosition(0, startPoint);
            FlashParticle(startPoint, dirFixed, maxdistance);
            RaycastHit2D hitTemp = Physics2D.Raycast(enemy.transform.position, dirFixed, maxdistance, obstacleMask);
            if (hitTemp.collider != null)
            {
                laserRenderer.SetPosition(1, hitTemp.point);
                HitParticle(hitTemp.point);
            }
            else
            {
                laserRenderer.SetPosition(1, (Vector2)enemy.transform.position + dirFixed * maxdistance);
                HitParticle((Vector2)enemy.transform.position + dirFixed * maxdistance);
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
        startPoint = enemy.transform.position + (Vector3)dir * (enemy.transform.localScale.x * 0.5f);
        laserRenderer.SetPosition(0, startPoint);
        FlashParticle(startPoint, dir, maxdistance);

        hit = Physics2D.Raycast(enemy.transform.position, dir, maxdistance, obstacleMask);

        if (hit.collider != null)
        {
            laserRenderer.SetPosition(1, hit.point);
            HitParticle(hit.point);
        }
        else
        {
            endPoint = (Vector2)enemy.transform.position + dir * maxdistance;
            laserRenderer.SetPosition(1, endPoint);
            HitParticle(endPoint);
        }

        if (endAngle - startAngle <= 0f)
        {
            LaserReset(enemy);
        }
    }

    private void FlashParticle(Vector2 position, Vector2 direction, float dis)
    {
        if (flashParticle == null)
        {
            flashParticle = Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedFlash);

            if (flashParticle == null) return;
            flashParticle.Play();
        }

        if (flashParticle.transform.position == (Vector3)position) return;

        flashParticle.transform.position = position;
        flashParticle.transform.rotation = Quaternion.LookRotation(direction);

        var flashmain = flashParticle.main;
        flashmain.startRotation = dis / flashmain.startSpeed.constant;
    }

    private void HitParticle(Vector2 position)
    {
        if (hitParticle != null)
        {
            if (hitParticle.transform.position == (Vector3)position)
                return;
            hitParticle.transform.position = position;
            return;
        }

        hitParticle = Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedHit);
        if (hitParticle == null) return;
        hitParticle.transform.position = position;
        hitParticle.Play();
    }



    public void LaserReset(Enemy enemy)
    {
        if (laserRenderer != null)
        {
            laserRenderer.enabled = false;
            laserRenderer.positionCount = 0; ;
            enemy.attackInterval = 0f;
            if(hitParticle != null)
            {
                Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedHit, hitParticle.gameObject);
                hitParticle = null;
            }
            
            if(flashParticle != null)
            {
                Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedFlash, flashParticle.gameObject);
                flashParticle = null;
            }
        }
        isInitialized = false;
    }
}
