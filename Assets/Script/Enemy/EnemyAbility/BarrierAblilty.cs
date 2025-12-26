using UnityEngine;

public class BarrierAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;

    public int maxBarrierAmount = 10000;
    public int barrierAmount;
    private bool active = true;

    public override bool isActive
    {
        get { return active; }
        set { active = value; }
    }
    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        barrierAmount = DataTableManager.OptionTable.GetValueDataToInt(5033);
    }
    public override int OnDamage(int damage)
    {
        if (!isActive) return damage;


        barrierAmount -= damage;
        Debug.Log("베리어 데미지 흡수 " + damage + ", 남은 베리어: " + barrierAmount);

        if (barrierAmount <= 0)
        {
            int overflowDamage = -barrierAmount;
            barrierAmount = 0;
            active = false;
            return overflowDamage;
        }
        return 0; 
    }

    public void RefillBarrier(int amount)
    {
        barrierAmount += amount;
#if DEBUG_MODE
        var text = enemy.textSpawnManager.SpawnTextUI(amount.ToString(), enemy.transform.position);
        text.SetColor(Color.green);
        Debug.Log($"베리어 리필이요{amount}");
#endif
        if (barrierAmount > maxBarrierAmount)
        {
            barrierAmount = maxBarrierAmount;
        }
        active = true;
    }
}