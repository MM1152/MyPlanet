using UnityEngine;
using System.Collections.Generic;
public class BaseElementalMove : IMove
{
  protected Dictionary<ElementType, IMove> moveStrategies = new Dictionary<ElementType, IMove>()
    {
        { ElementType.Normal, new SimpleMove() },
        { ElementType.Fire, new UpDownMove() },
        { ElementType.Ice, new LeftRinghMove() },
        { ElementType.Steel, new LeftRinghMove() },
        { ElementType.Light, new LeftRinghMove() },
        { ElementType.Dark, new CornerWrapMove() },
    };  
    public IMove currentStrategy {get; protected set;}

    public virtual void Init(Enemy enemy)
    {
        if (moveStrategies.TryGetValue(enemy.ElementType, out var strategy))
        {
            currentStrategy = strategy;        
            currentStrategy.Init(enemy);
        }
        else
        {
            Debug.LogWarning($"[Move] 전략 없음: {enemy.ElementType}");
        }   
    }

    public void Move(Enemy enemy)
    {
        currentStrategy?.Move(enemy);
    }
}
