using System;
using System.Collections.Generic;
using UnityEngine;

public class DivergentBarrier : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;
    public int barrierAmount = 1000;
    public ElementType barrierElementType = ElementType.Ice;
    public override bool isActive { get; set; }
    public Barrier barrier;

    private int refillCount = 2;

    private int[] refillSteps = new int[] { 50, 25 };
    private HashSet<int> refillStepHold = new HashSet<int>();



    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        Debug.Log("DivergentBarrier SetEnemy 호출");
        CreateBarrier();
        enemy.OnBarrierRefill += BarrierRefill;

    }
    int num = 0;
    private void CreateBarrier()
    {
        isActive = true;
        Debug.Log($"베리어 생성 숫자 {num++}");
        var barrierObj = Managers.ObjectPoolManager.SpawnObject<Barrier>(PoolsId.Barrier);
        barrierObj.transform.SetParent(enemy.transform);
        barrierObj.transform.localPosition = Vector3.zero;
        barrier = barrierObj.GetComponent<Barrier>();
        barrier.Init(barrierAmount, barrierElementType, enemy, OnBarrierDestroyed);
        refillCount = 2;
        refillStepHold.Clear();
    }

    private void OnBarrierDestroyed(int overflowBaseDamage, ElementType attackerType)
    {

        isActive = false; // 능력 비활성화

        if (overflowBaseDamage > 0) // 남은 데미지가 있으면 적에게 전달
        {
            float percent = TypeEffectiveness.StaticGetDamagePercent(attackerType, enemy.TypeEffectiveness.Type);
            Debug.Log($"오버한 데미지 {overflowBaseDamage}, 공격자타입: {attackerType}, 최종 데미지 보정값: {percent}");
            int finalDamage = Mathf.Clamp(Mathf.RoundToInt(overflowBaseDamage * percent), 1, int.MaxValue);
            enemy.currentHP -= finalDamage;
            Debug.Log($"최종데미지 {finalDamage}, 적 남은체력: {enemy.currentHP}");
            if (enemy.currentHP <= 0) // 적 사망 처리
            {
                enemy.OnDead();
            }
        }
        barrier.gameObject.SetActive(false);
        Debug.Log($"베리어 활성화 상태 {barrier.gameObject.activeSelf}");
        Debug.Log("베리어 파괴 후 적 데미지 처리");
    }

    private void BarrierRefill(int amount)
    {
        if (refillCount <= 0) return;
        if (barrier == null) return;

        foreach (var step in refillSteps)
        {
            if (amount <= step && !refillStepHold.Contains(step))
            {
                refillStepHold.Add(step);

                // barrier?.RefillBarrier(barrierAmount);
                 barrier?.RefillBarrier(999999);
                refillCount--;
                isActive = true;
                break;
            }
        }
    }

    public override int OnDamage(int damage)
    {
        if (!isActive) return damage;

        barrier.OnDamage(damage);

        return -1;
    }
}