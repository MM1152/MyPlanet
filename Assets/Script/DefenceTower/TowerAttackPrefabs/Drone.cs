using System;
using UnityEngine;

public class Drone : BaseAttackPrefab
{
    public event Action<Drone> OnDead;
    private Vector3 endPos;
    private Vector3 dir;
    private Rect rect;

    private float speed = 3f;
    private float findNextPosDistance = 0.5f;
    private int hp;
    public override void Init(Tower tower)
    {
        base.Init(tower);
        poolsId = PoolsId.Drone;
        rect = Screen.safeArea;

        var randX = UnityEngine.Random.Range(rect.xMin, rect.xMax);
        var randY = UnityEngine.Random.Range(rect.yMin, rect.yMax);
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(randX, randY, -Camera.main.transform.position.z));
        endPos = new Vector3(pos.x  , pos.y);
        dir = (endPos - transform.position).normalized;
        hp = tower.BonusDroneHp;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagIds.EnemyTag))
        {
            var enemy = collision.GetComponent<Enemy>();
            if(enemy == null)
                return;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += dir * speed * Time.deltaTime;

        if(Vector3.Distance(endPos , transform.position) < findNextPosDistance)
        {
            var randX = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var randY = UnityEngine.Random.Range(rect.yMin, rect.yMax);
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(randX, randY, -Camera.main.transform.position.z));
            endPos = new Vector3(pos.x, pos.y);
            dir = (endPos - transform.position).normalized;
        }

        if(transform.position.x < rect.xMin - 1f || transform.position.x > rect.xMax + 1f ||
           transform.position.y < rect.yMin - 1f || transform.position.y > rect.yMax + 1f)
        {
            var randX = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var randY = UnityEngine.Random.Range(rect.yMin, rect.yMax);
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(randX, randY, -Camera.main.transform.position.z));
            endPos = new Vector3(pos.x, pos.y);
            dir = (endPos - transform.position).normalized;
        }
    }

    public void ForceDead()
    {
        OnDead?.Invoke(this);
        if(gameObject.activeSelf)
            Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }
}