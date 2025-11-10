using System;

public interface IPassive
{
    public Action OnInit { get; set; }
    public Action OnAttack { get; set; }
    public Action OnHit { get; set; }

    public void Init(TowerManager towerManager);
    public void ApplyPassive();
}