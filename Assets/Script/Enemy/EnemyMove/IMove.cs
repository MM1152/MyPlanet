using UnityEngine;

public interface IMove
{
    public void Init(Enemy enemy);
    public void Move(Enemy enemy);
    public Vector2 Direction { get; }
}
