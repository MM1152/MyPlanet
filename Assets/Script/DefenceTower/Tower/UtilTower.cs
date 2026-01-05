using System.Threading;
using UnityEngine;

public class UtilTower : Tower
{
    private TowerTable.UtilTower utiltowerData;
    public TowerTable.UtilTower UtilTowerData => utiltowerData;
    protected Transform planet;
    protected Transform defenseTower;
    private float FullCoolTime => (utiltowerData?.Cooltime ?? 0) + BonusCoolTime;
    public float FullDuration => utiltowerData.Duration + BonusDuration;
    protected float timer = 0f;

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
        utiltowerData = data as TowerTable.UtilTower;

        planet = GameObject.FindWithTag(TagIds.PlayerTag).transform;
        defenseTower = GameObject.FindWithTag(TagIds.DefenseTowerTag).transform;
    }

    public override void Update(float deltaTime)
    {
        if (!UseAble) return;

        timer += deltaTime;
        if (FullCoolTime <= timer)
        {
            timer = 0;
            Attack();
        }
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Debug.Log("��ӹ޾Ƽ� �����");
        return null;
    }
}
