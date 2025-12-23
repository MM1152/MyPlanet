using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class FortifiedBarrierAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;
    public int maxBarrierAmount = 3000;// 임시값
    public int barrierAmount;
    private float refillTimer = 5f; //임시 
    public override bool isActive => barrierAmount > 0;
    private float reductionDamage = 0.4f; //테이블연동필요 //피해감소율 
#if DEBUG_MODE
    TestRange rangePrefab;
    bool setSprite = false;
#endif

    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        barrierAmount = DataTableManager.OptionTable.GetValueDataToInt(5033);
        maxBarrierAmount = barrierAmount;
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
            // var spr = rangePrefab.GetComponent<SpriteRenderer>();
            // spr.color = enemy.spriteRenderer.color;
            // spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.5f);
            float radius = enemy.transform.localScale.x;
            float visualScale = radius * 10f;
            rangePrefab.transform.localScale = new Vector3(visualScale, visualScale, 1f);
        }
#endif

        if (barrierAmount > 0)
        {
            var reduceDamageWithBarrier = damage * reductionDamage;
            barrierAmount -= (int)reduceDamageWithBarrier;
        }
        Debug.Log($"베리어 데미지 흡수 {damage}, 남은 베리어: {barrierAmount}");

        if (barrierAmount <= 0)
        {
            Debug.Log("베리어 파괴! 타이머 시작");
            StartRefillTimer().Forget();
            int overflowDamage = (int)(-barrierAmount / reductionDamage);
            barrierAmount = 0;

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

    }

    private async UniTaskVoid StartRefillTimer()
    {
        if (enemy == null) return;
        await UniTask.Delay(System.TimeSpan.FromSeconds(refillTimer), ignoreTimeScale: false, cancellationToken: enemy.GetCancellationTokenOnDestroy());
        RefillBarrier(maxBarrierAmount);

#if DEBUG_MODE
        if (rangePrefab != null)
        {
            rangePrefab.gameObject.SetActive(true);
            float radius = enemy.transform.localScale.x;
            float visualScale = radius * 10f;
            rangePrefab.transform.localScale = new Vector3(visualScale, visualScale, 1f);
        }
#endif
    }
}
