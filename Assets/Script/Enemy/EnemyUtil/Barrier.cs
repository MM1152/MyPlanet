using UnityEngine;
using System;
using Unity.VisualScripting;

public class Barrier : MonoBehaviour
{
    public int barrierAmount;
    private int maxBarrierAmount;
    // public StatusEffect StatusEffect => null; // 방어막에는 상태이상 없음
    public bool IsDead => isDead; // 방어막 유무 
    private bool isDead = false; // 방어막 파괴 여부
    public ElementType ElementType => elementType; // 방어막 속성
    private TypeEffectiveness typeEffectiveness = new TypeEffectiveness(); // 방어막 타입상성
    private ElementType elementType = ElementType.Ice; // 기본 속성 얼음
    private Enemy enemy; // 방어막이 붙어있는 적
    private Action<int, ElementType> onBarrierDestroyed;
    // 데미지 넘겨줘야함, 마지막 타입 넘겨줘야함 
    public void Init(int value, ElementType type, Enemy enemy, Action<int, ElementType> destroyCallback = null)
    {
        barrierAmount = value;
        maxBarrierAmount = barrierAmount;
        elementType = type;
        this.enemy = enemy;
        typeEffectiveness.Init(elementType);
        onBarrierDestroyed = destroyCallback;
        isDead = false;
        if (enemy != null)
            enemy.OnDie += OnDead;
    }

    public void OnDamage(int damage)
    {
        if (isDead) return;

        if (elementType != enemy.LastAttackerType)//속성 상성이 다르면 데미지 재조정
        {
            damage = RemapDamageToBarrier(damage);
        }
        barrierAmount -= damage;

        Debug.Log($"Barrier took {damage} damage, 남아있는 베리어값: {barrierAmount}");
        Debug.Log($"마지막 공격자 타입: {enemy.LastAttackerType}");
        Debug.Log($"속성 가중치 {TypeEffectiveness.StaticGetDamagePercent(enemy.LastAttackerType, elementType)}");

        if (barrierAmount <= 0)
        {
            int overflowBaseDamage = -barrierAmount;
            Debug.Log($"베리어값 오버한 데미지: {overflowBaseDamage}");
            barrierAmount = 0;
            isDead = true;
            float barrierMultiplier = TypeEffectiveness.StaticGetDamagePercent(enemy.LastAttackerType, elementType);
            Debug.Log($"마지막 공격타입 {enemy.LastAttackerType}, 베리어타입 {elementType}");
            Debug.Log($"베리어 멀티플라이어: {barrierMultiplier}, 마지막 공격자 타입: {enemy.LastAttackerType}");
            int baseDamage = Mathf.RoundToInt(overflowBaseDamage / barrierMultiplier);
            Debug.Log($"보정뺀 데미지{baseDamage}, 공격자타입: {enemy.LastAttackerType}");
            onBarrierDestroyed?.Invoke(baseDamage, enemy.LastAttackerType);
        }

        if (isDead)
        {
            this.GameObject().SetActive(false);
        }
    }

    private int RemapDamageToBarrier(int damage)
    {
        var attackerType = enemy.LastAttackerType;
        float attackerToEnemy = TypeEffectiveness.StaticGetDamagePercent(attackerType, enemy.ElementType);
        float attackerToBarrier = TypeEffectiveness.StaticGetDamagePercent(attackerType, elementType);
        int baseDamage = attackerToEnemy > 0.0001f ? Mathf.Max(1, Mathf.RoundToInt(damage / attackerToEnemy)) : damage;
        int newDamage = Mathf.Max(1, Mathf.RoundToInt(baseDamage * attackerToBarrier));
        return newDamage;
    }

    public void RefillBarrier(int amount)
    {
        barrierAmount += amount;

        // if (barrierAmount > maxBarrierAmount)
        // {
        //     barrierAmount = maxBarrierAmount;
        // }

        if (isDead && barrierAmount > 0)
        {
            isDead = false;
            this.gameObject.SetActive(true);
            Debug.Log("베리어가 리필되어 다시 활성화되었습니다.");
            Debug.Log($"현재 베리어값: {barrierAmount}");
        }

    }

    public void OnDead(Enemy enemy)
    {
        if (enemy == this.enemy)
        {
            Destroy(this.gameObject);
        }
    }
}
