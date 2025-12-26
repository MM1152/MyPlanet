using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class FortifiedBarrierAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;
    public int maxBarrierAmount = 5000;// 임시값
    public int barrierAmount;
    private float refillTimer = 5f; //임시 
    public override bool isActive => barrier == null || barrier.IsDead; // 베리어가 없을 때 활성화
    private Barrier barrier;  // 풀링해서 가져와서 담아둘 베리어
    private float reductionDamage = 0.4f; //피해감소율 설정테이블 연결필요 


    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        barrierAmount = DataTableManager.OptionTable.GetValueDataToInt(5033);        
        maxBarrierAmount = barrierAmount;
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
        var barrierObj = Managers.ObjectPoolManager.SpawnObject<Barrier>(PoolsId.SteelBarrier);
        Debug.Log("베리어 오브젝트 풀에서 스폰 완료");
        Debug.Log($"베리어 오브젝트 이름: {barrierObj.name}");
        barrierObj.transform.SetParent(enemy.transform);
        barrierObj.transform.localPosition = Vector3.zero;
        barrier = barrierObj.GetComponent<Barrier>();
        barrier.Init(barrierAmount, enemy.ElementType, enemy, OnBarrierDestroyed, reductionDamage);
        StartRefillTimer().Forget();
    }
    public override int OnDamage(int damage)
    {
        if (isActive) return damage;

        Debug.Log("베리어가 없어서 읽히면 안되는데 여기로 옴");
        return 0; // 데미지 0으로 처리해서 베리어가 다 흡수하게 함
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


    public void RefillBarrier(int amount)
    {
        if (barrier == null) return;

        if (barrier.gameObject.activeSelf == false)
        {
            barrier.gameObject.SetActive(true);
        }

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
        while (enemy != null && enemy.gameObject.activeSelf)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(refillTimer), ignoreTimeScale: false, cancellationToken: enemy.GetCancellationTokenOnDestroy());
            RefillBarrier(maxBarrierAmount);
        }
    }

       private void Barrier_OnDead(Enemy enemy)
    {
         barrier.transform.SetParent(null);
         Managers.ObjectPoolManager.Despawn(PoolsId.SteelBarrier, barrier.gameObject); 
    }
}
