using UnityEngine;

public enum EnemyMovePattern
{
    ToAttackPoint,
    Attacking,
    ReturnPoint,
    Waiting,
}

public interface IMove
{
    public void Init(Enemy enemy);
    public void Move(Enemy enemy);
}
