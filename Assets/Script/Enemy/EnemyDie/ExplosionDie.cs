using UnityEngine;

public class ExplosionDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Player" };

    private int explosionAtk;
    private int SetAtk()
    {
        return enemy.ElementType switch
        {
            ElementType.Fire => DataTableManager.OptionTable.GetValueDataToInt(5030),
            _ => explosionAtk = 0,
        };
    }

    protected override void DieAbility(Collider2D collider)
    {
        var find = collider.GetComponent<IDamageAble>();
        explosionAtk = SetAtk();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage(Mathf.Clamp((int)((explosionAtk - find.Defense) * percent), 1, int.MaxValue));
            Debug.Log($"{enemy.ElementType} Explosion Damage: {Mathf.Clamp((int)((explosionAtk - find.Defense) * percent), 1, int.MaxValue)}");
        }
    }
}
