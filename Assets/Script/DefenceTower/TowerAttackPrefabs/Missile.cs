using UnityEngine;
using System;

public class Missile : ProjectTile
{
    [Header("Missile Setting")]
    [SerializeField] private float roatationSpeed = 50f;
    [SerializeField] private float trackingStrength = 0.5f;
    private float currentAngle;
    private float lookAngleNoise = 0f;

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Missile;
    }

    public override void SetTarget(Transform target, float minNoise , float maxNoise)
    {
        base.SetTarget(target, minNoise , maxNoise);


        Vector3 dir = target.position - transform.position;
        float noise = UnityEngine.Random.Range(minNoise , maxNoise);
        float lookAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + UnityEngine.Random.Range(-noise, noise);
        transform.eulerAngles = new Vector3(0f, 0f, lookAngle);
            
        currentAngle = transform.eulerAngles.z;
    }

    protected override Vector3 SetDir()
    {
        Vector3 targetDirection = target.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        float maxRotation = roatationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -maxRotation, maxRotation);

        rotationAmount *= trackingStrength;
        currentAngle += rotationAmount;

        float radAngle = currentAngle * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(radAngle) , Mathf.Sin(radAngle) , 0).normalized;
    }

    protected override void Update()
    {
        base.Update();

        dir = SetDir();
        transform.eulerAngles = new Vector3(0f , 0f , currentAngle);
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        Managers.ObjectPoolManager.Despawn(PoolsId.Missile, this.gameObject);
        //Destroy(gameObject);
    }
}