using UnityEngine;

public class IronMineExplosion : Explosion
{
    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag(TagIds.IronMineTag))
        {
            var findMine = collision.GetComponent<IronMine>();
            if (findMine != null)
            {
                findMine.ForcingBoom();
            }
        }
    }
}