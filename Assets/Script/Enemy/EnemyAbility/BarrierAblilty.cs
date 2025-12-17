using UnityEngine;

public class BarrierAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;

    public int maxBarrierAmount = 10000;
    public int barrierAmount;
    private bool active = true;

#if DEBUG_MODE
    TestRange rangePrefab;
    bool setSprite = false;
#endif
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

#if DEBUG_MODE
        if (!setSprite)
        {
            setSprite = true;
            rangePrefab = Managers.ObjectPoolManager.SpawnObject<TestRange>(PoolsId.TestRange);
            rangePrefab.transform.SetParent(enemy.transform);
            rangePrefab.transform.position = enemy.transform.position;
            var spr = rangePrefab.GetComponent<SpriteRenderer>();
            spr.color = enemy.spriteRenderer.color;
            float radius = enemy.transform.localScale.x;
            float visualScale = radius * 10f;
            rangePrefab.transform.localScale = new Vector3(visualScale, visualScale, 1f);
        }
#endif

        barrierAmount -= damage;
        Debug.Log("베리어 데미지 흡수 " + damage + ", 남은 베리어: " + barrierAmount);

        if (barrierAmount <= 0)
        {
            int overflowDamage = -barrierAmount;
            barrierAmount = 0;
            active = false;
#if DEBUG_MODE
            rangePrefab.gameObject.SetActive(false);
#endif
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