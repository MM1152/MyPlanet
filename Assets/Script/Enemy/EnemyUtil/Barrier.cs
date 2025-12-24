using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Barrier : MonoBehaviour, IDamageAble
{
    public int barrierAmount;
    public bool IsBarrier => barrierAmount > 0;
    public bool IsDead => barrierAmount <= 0;
    public ElementType ElementType => elementType; // 방어막 속성 생성시 할당 
    public StatusEffect StatusEffect => null; // 방어막에는 상태이상 없음
    private TypeEffectiveness typeEffectiveness = new TypeEffectiveness(); // 방어막 타입상성 초기화시 필요 
    private ElementType elementType = ElementType.Ice; // 기본 속성 얼음
    private Action<int, ElementType> onBarrierDestroyed;
    private TypeEffectiveness targetTypeEffectiveness = new TypeEffectiveness();
    private float reductionDamage; //피해감소율
    public CircleCollider2D Collider => barrierCollider;
    [SerializeField]
    private CircleCollider2D barrierCollider;


    public void Init(int value, ElementType type, Enemy enemy, Action<int, ElementType> destroyCallback = null, float reductionDamage = 0f)
    {
        barrierAmount = value;
        elementType = type;
        typeEffectiveness.Init(elementType);
        onBarrierDestroyed = destroyCallback;
        this.gameObject.SetActive(true);
        Debug.Log($"베리어 초기화 완료. 값: {barrierAmount}, 속성: {elementType}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        var tower = collision.GetComponent<BaseAttackPrefab>()?.Tower;
        if (tower == null) return;
        targetTypeEffectiveness.Init((ElementType)tower.TowerData.attribute);
    }

    public void OnDamage(int damage)
    {
        // 타워에서 데미지 계산해서 넘겨줄 때 사용 
        if (IsDead) return;
        
        if (reductionDamage > 0)
        {
            Debug.Log($"원래 데미지: {damage}");
            Debug.Log($"피해감소율: {reductionDamage}");
            damage = (int)(damage * (1f - reductionDamage));
            Debug.Log($"피해감소율 적용된 데미지: {damage}");
        }

        barrierAmount -= damage;
        Debug.Log($"베리어가 {damage}만큼 데미지를 받았습니다. 남은 베리어값: {barrierAmount}");
        if (barrierAmount <= 0)
        {
            int overflowBaseDamage = -barrierAmount; // 베리어가 흡수하지 못한 데미지
            if(reductionDamage > 0)
            {
                Debug.Log($"감소된 데미지를 역산하기전 베리어 오버 데미지: {overflowBaseDamage}");
                Debug.Log($"피해감소율: {reductionDamage}");
                overflowBaseDamage = (int)(overflowBaseDamage / (1f - reductionDamage));
                Debug.Log($"감소된 데미지를 역산한 베리어 오버 데미지: {overflowBaseDamage}");
            }
            Debug.Log($"베리어값 오버한 데미지: {overflowBaseDamage}");
            barrierAmount = 0; // 베리어값 0으로 고정
            float reversePercent = typeEffectiveness.GetReverseDamagePercent(targetTypeEffectiveness.Type, elementType);
            int reverseDamage = Mathf.Clamp((int)(overflowBaseDamage * reversePercent), 1, int.MaxValue);
            Debug.Log($"베리어 파괴시 역산된 베리어 오버 데미지: {reverseDamage}");
            onBarrierDestroyed?.Invoke(reverseDamage, targetTypeEffectiveness.Type); // 이거 에너미가아니라 베리어의 마지막 공격자타입으로 수정 , 상대방의 타입보내줌 
            OnDead();
        }
    }

    public void RefillBarrier(int amount)
    {
        barrierAmount += amount;

        if (barrierAmount > 0)
        {
            this.gameObject.SetActive(true);
            Debug.Log("베리어가 리필되어 다시 활성화되었습니다.");
            Debug.Log($"현재 베리어값: {barrierAmount}");
        }
    }

    public void OnDead()
    {
        this.gameObject.SetActive(false);
    }
}
