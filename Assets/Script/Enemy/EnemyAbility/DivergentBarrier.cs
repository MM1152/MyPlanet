using System;
using System.Collections.Generic;
using UnityEngine;

public class DivergentBarrier : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;
    public int barrierAmount;
    private List<float> barrierPercents = new List<float>();
    private int barrierPercentIndex = 0;
    public ElementType barrierElementType = ElementType.Ice;
    public override bool isActive => barrier == null || (refillCount > 0 && barrier.IsDead);
    public Barrier barrier;
    private int refillCount = 2; // 회복 카운트 => 설정테이블 연결 

    private int[] refillSteps = new int[] { 50, 25 }; // 체력 퍼센트 기준 후에 수정? 
    private HashSet<int> refillStepHold = new HashSet<int>(); // 이미 리필된 스텝들
    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        for (int i = 5075; i <= 5075 + refillSteps.Length; i++)
        {
            float percent = DataTableManager.OptionTable.GetValueDataToFloat(i);
            barrierPercents.Add(percent);
        }
        enemy.OnBarrierRefill += AbilityBarrierRefill;
        enemy.OnDie += Barrier_OnDead;
        CreateBarrier();

    }

    private void CreateBarrier()
    {
        if (barrier != null && barrier.gameObject != null && barrier.gameObject.activeSelf)
        {
            Debug.Log("기존 활성 베리어가 있어 새로 생성하지 않습니다.");
            return;
        }
        var barrierObj = Managers.ObjectPoolManager.SpawnObject<Barrier>(PoolsId.IceBarrier);
        Debug.Log("베리어 오브젝트 풀에서 스폰 완료");
        Debug.Log($"베리어 오브젝트 이름: {barrierObj.name}");
        barrierObj.transform.SetParent(enemy.transform);
        barrierObj.transform.localPosition = Vector3.zero;
        barrier = barrierObj.GetComponent<Barrier>();
        barrier.Init(barrierAmount, barrierElementType, enemy, OnBarrierDestroyed);
        barrierPercentIndex = 0;
        refillCount = 2;
        refillStepHold.Clear();
    }

    private void OnBarrierDestroyed(int overflowBaseDamage, ElementType attackerType)
    {
        Debug.Log("베리어가 파괴되어 적 데미지 처리 시작");
        // 파괴될때 베리어에서 오바된 데미지값 넘겨서 실행할 함수
        if (overflowBaseDamage > 0) // 남은 데미지가 있으면 적에게 전달
        {
            Debug.Log("베리어 파괴 후 적에게 오버한 데미지 전달");

            float damagePercent = enemy.typeEffectiveness.GetDefenderDamagePercent(attackerType);
            float finalDamage = overflowBaseDamage * damagePercent;
            enemy.currentHP -= (int)finalDamage;
            Debug.Log($"최종데미지 {finalDamage}, 적 남은체력: {enemy.currentHP}");
            if (enemy.currentHP <= 0) // 적 사망 처리
            {
                barrier.transform.SetParent(null); // 베리어 분리
                Managers.ObjectPoolManager.Despawn(PoolsId.IceBarrier, barrier.gameObject); // 베리어 디스폰
                enemy.OnDead();
            }
        }
        Debug.Log($"베리어 활성화 상태 {barrier.gameObject.activeSelf}");
        Debug.Log("베리어 파괴 후 적 데미지 처리");
    }

    private void AbilityBarrierRefill(int amount)
    {
        if (refillCount <= 0) return;
        if (barrier == null) return;
        if (!barrier.IsDead) return;
        Debug.Log($"베리어 리필 체크 중. 현재 체력 퍼센트: {amount}%, 남은 리필 횟수: {refillCount}");
        foreach (var step in refillSteps)
        {
            if (amount <= step && !refillStepHold.Contains(step))
            {
                Debug.Log($"베리어 리필 트리거됨. 현재 체력 퍼센트: {amount}%, 리필 스텝: {step}%, 남은 리필 횟수: {refillCount}");
                refillStepHold.Add(step);
                var refill = enemy.MaxHp * barrierPercents[barrierPercentIndex];
                barrier?.RefillBarrier((int)refill);
                barrierPercentIndex++;
                refillCount--;
                break;
            }
        }
    }

    public override int OnDamage(int damage)
    {
        if (isActive) return damage;

        Debug.Log("베리어가 없어서 읽히면 안되는데 여기로 옴");
        return 0; // 데미지 0으로 처리해서 베리어가 다 흡수하게 함
    }

    private void Barrier_OnDead(Enemy enemy)
    {
        barrier.transform.SetParent(null);
        Managers.ObjectPoolManager.Despawn(PoolsId.IceBarrier, barrier.gameObject);
    }
}