using UnityEngine;
using System.Collections.Generic;
public class BaseBossMonsterMove : IMove
{
    protected Dictionary<int, IMove> moveStrategies = new Dictionary<int, IMove>()
    {
        { 0, new BossSimpleMove() },
        {3067, new OneTwoMoveAttack() },
    };
    public IMove currentStrategy { get; protected set; }

    public Vector2 Direction => currentStrategy?.Direction ?? Vector2.zero;

    public virtual void Init(Enemy enemy)
    {
        if (moveStrategies.TryGetValue(enemy.enemyData.ID, out var strategy))
        {
            currentStrategy = strategy;
            currentStrategy.Init(enemy);
        }
        else
        {
            currentStrategy = moveStrategies[0];
            currentStrategy.Init(enemy);
        }
    }

    public void Move(Enemy enemy)
    {
        currentStrategy?.Move(enemy);
    }
}
