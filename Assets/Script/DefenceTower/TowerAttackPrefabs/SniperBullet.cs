using UnityEngine;
using System;

public class SniperBullet : ProjectTile
{
    [SerializeField] private float durationTime = 0.5f;

    private TrailRenderer trailRenderer;
    private float currentDurationTime;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.SniperBullet;
        speed = 15f;

        currentDurationTime = durationTime;
        trailRenderer.SetPosition(0 ,transform.position);
    }

    protected override void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        currentDurationTime -= Time.deltaTime;
        
        if (currentDurationTime <= 0)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }
}