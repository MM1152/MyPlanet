using UnityEngine;

public class PlutoBullet : Bullet
{
    private float passiveDamagePercent;

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.PlutoBullet;
    }

    public void SetPassiveData(float passiveDamagePercent)
    {
        this.passiveDamagePercent = passiveDamagePercent;
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();

        if(find != null)
        {
            find.OnDamage((int)(tower.CalcurateAttackDamage * (passiveDamagePercent / 100f)));
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId , gameObject);
        }
    }
}