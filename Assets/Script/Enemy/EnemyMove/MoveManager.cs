 using System.Collections.Generic;
using UnityEngine;

public class MoveManager 
{
  private Dictionary<int, IMove> moveTable = new Dictionary<int, IMove>()
    {        
        { 0, new SimpleMove() },                
        { 4026, new UpDownMove() },
        { 5026, new LeftRinghMove() },
        { 3026, new LeftRinghMove() },
        { 6026, new LeftRinghMove() },
        { 7026, new CornerWrapMove() },
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
