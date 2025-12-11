using UnityEngine;

public class GravityField : BaseAttackPrefab
{
    public override void Init(Tower data)
    {
        base.Init(data);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void HitTarget(Collider2D collision)
    {
        throw new System.NotImplementedException();
    }
}