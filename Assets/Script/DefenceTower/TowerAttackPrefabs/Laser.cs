using UnityEngine;
using System;

public class Laser : ProjectTile
{
    
    [Header("Laser Settings")]
    // 레이저 지속시간
    [SerializeField] private float laserDurtaionTime = 0.5f;

    private CapsuleCollider2D collder;
    // 레이저 작아지는 값
    private float decreaseAmount;

    // 레이저 작아지는 현재 타이밍
    private float currentDuration;
    private float currentSize;

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Laser;
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
    }

    public override void SetTarget(Transform target , float minNoise , float maxNoise)
    {
        base.SetTarget(target , minNoise, maxNoise);

        collder = GetComponent<CapsuleCollider2D>();

        // 레이저 크기 설정
        Vector3 linear = target.position - transform.position;
        transform.localScale =
            new Vector3(
                linear.magnitude,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z
            );
        collder.direction = CapsuleDirection2D.Horizontal;
        collder.size = new Vector2(linear.magnitude, 1f);

        // 레이저 위치 설정
        transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);

        // 레이저 크기 줄이는 기본 설정
        currentSize = gameObject.transform.localScale.y;
        currentDuration = 0f;
        decreaseAmount = currentSize / laserDurtaionTime * Time.deltaTime;
    }

    protected override void Update()
    {
        // 레이저 크기 줄이기
        if(Time.timeScale > 0)
        {
            currentDuration += Time.deltaTime;
            currentSize -= decreaseAmount;
            transform.localScale = new Vector3(
                transform.localScale.x,
                currentSize,
                1f
                );

            if (currentDuration >= laserDurtaionTime)
            {
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
                return;
            }
        }
    }


}