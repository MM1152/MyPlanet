 using System.Collections.Generic;
using UnityEngine;

public class MoveManager 
{
  private Dictionary<int, IMove> moveTable = new Dictionary<int, IMove>()
    {
        { 0, new SimpleMove() },
        { 1, new LeftRinghMove() },
        { 2, new UpDownMove() },
        { 3026, new CornerWrapMove() },
    };

    public IMove GetMove(int Id)
    {
        if (moveTable.ContainsKey(Id))
        {
            return moveTable[Id];
        }
        return moveTable[0];    
    }
}
