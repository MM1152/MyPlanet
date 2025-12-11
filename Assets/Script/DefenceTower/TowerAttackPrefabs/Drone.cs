using System;
using Unity.VisualScripting;
using UnityEngine;

public class Drone : BaseAttackPrefab , IDamageAble
{
    public event Action<Drone> OnDie;
    private Vector3 endPos;
    private Vector3 dir;
    private Rect worldRect; // 월드 좌표계로 변환된 rect

    private float speed = 3f;
    private float findNextPosDistance = 0.1f;
    private int hp;
    private bool isDead;
    public StatusEffect StatusEffect => throw new NotImplementedException();
    public bool IsDead => isDead;
    public ElementType ElementType => ElementType.Normal;

    public override void Init(Tower tower)
    {
        base.Init(tower);
        poolsId = PoolsId.Drone;
        
        // Screen.safeArea를 월드 좌표로 변환
        var screenRect = Screen.safeArea;
        var bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(screenRect.xMin, screenRect.yMin, -Camera.main.transform.position.z));
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(screenRect.xMax, screenRect.yMax, -Camera.main.transform.position.z));
        
        worldRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);

        var randX = UnityEngine.Random.Range(worldRect.xMin, worldRect.xMax);
        var randY = UnityEngine.Random.Range(worldRect.yMin, worldRect.yMax);
        endPos = new Vector3(randX, randY);
        dir = (endPos - transform.position).normalized;
        hp = tower.BonusDroneHp;
        isDead = false;
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
            var randX = UnityEngine.Random.Range(worldRect.xMin, worldRect.xMax);
            var randY = UnityEngine.Random.Range(worldRect.yMin, worldRect.yMax);
            endPos = new Vector3(randX, randY);
            dir = (endPos - transform.position).normalized;
        }

        if(transform.position.x < worldRect.xMin - 1f || transform.position.x > worldRect.xMax + 1f ||
           transform.position.y < worldRect.yMin - 1f || transform.position.y > worldRect.yMax + 1f)
        {
            var randX = UnityEngine.Random.Range(worldRect.xMin, worldRect.xMax);
            var randY = UnityEngine.Random.Range(worldRect.yMin, worldRect.yMax);
            endPos = new Vector3(randX, randY);
            dir = (endPos - transform.position).normalized;
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }

    public void OnDamage(int damage)
    {
        hp -= damage;

        if( hp <= 0 )
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        OnDie?.Invoke(this);
        isDead = true;
        if (gameObject.activeSelf)
            Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
    }
}