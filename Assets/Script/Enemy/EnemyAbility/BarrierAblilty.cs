using UnityEngine;

public class BarrierAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;

    public int barrierAmount = 100;

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
            // spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.5f);
            float radius = enemy.transform.localScale.x;
            float visualScale = radius * 10f;
            rangePrefab.transform.localScale = new Vector3(visualScale, visualScale, 1f);
        }
#endif

        barrierAmount -= damage;
#if DEBUG_MODE
        Debug.Log($"BarrierAbility: Barrier remaining {barrierAmount}");
#endif

        if (barrierAmount < 0)
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
}
