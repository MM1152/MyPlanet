using System.Threading.Tasks;
using UnityEngine;

public class LaserShot : IShotStrategy
{
    private bool isInitialized = false;
    private float offset = 0.1f;
    private LineRenderer lineRenderer;
    private RaycastHit2D hit;
    private LayerMask obstacleMask;
    private Vector2 startPoint;
    private ParticleSystem hitParticle;
    private ParticleSystem flashParticle;

    public void Shot(Enemy enemy, GameObject target)
    {
        if (target == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        if (hit.collider == null) return;

        var find = hit.collider.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = enemy.TypeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage(Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue));
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
            obstacleMask = LayerMask.GetMask("DefenseTower", "Player");
            enemy.OnDie += LaserReset;
        }

        Vector2 dir = (target.transform.position - enemy.transform.position).normalized;
        float dis = Vector2.Distance(enemy.transform.position, target.transform.position);

        hit = Physics2D.Raycast(enemy.transform.position, dir, dis);
        startPoint = enemy.transform.position + (Vector3)dir * (enemy.transform.localScale.x * 0.5f);
        lineRenderer.SetPosition(0, startPoint);
        FlashParticle(startPoint, dir, dis);
        hit = Physics2D.Raycast(startPoint, dir, dis, obstacleMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(target.tag) || hit.collider.CompareTag(TagIds.PlayerTag)
                || hit.collider.CompareTag(TagIds.DefenseTowerTag))
            {
                lineRenderer.SetPosition(1, hit.collider.transform.position);
                HitParticle(hit.point);
            }
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
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            if (hitParticle != null) Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedHit, hitParticle.gameObject);
            if (flashParticle != null) Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedFlash, flashParticle.gameObject);
            hitParticle = null;
            flashParticle = null;
        }
        isInitialized = false;
    }
}

