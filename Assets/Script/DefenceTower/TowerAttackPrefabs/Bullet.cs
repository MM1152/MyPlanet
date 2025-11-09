using UnityEngine;

public class Bullet : ProjectTile
{
    protected override void HitTarget()
    {
        base.HitTarget();
        Destroy(gameObject);
    }
}