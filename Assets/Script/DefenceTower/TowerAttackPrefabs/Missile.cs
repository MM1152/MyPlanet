using UnityEngine;

public class Missile : ProjectTile
{
    [Header("Missile Setting")]
    [SerializeField] private float roatationSpeed = 50f;
    [SerializeField] private float trackingStrength = 0.5f;
    private float currentAngle;

    public override void Init(TowerData.Data data , TypeEffectiveness typeEffectiveness)
    {
        base.Init(data , typeEffectiveness);
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

    protected override void HitTarget()
    {
        base.HitTarget();
        Destroy(gameObject);
    }
}