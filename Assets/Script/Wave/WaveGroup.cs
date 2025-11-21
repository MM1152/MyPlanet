using UnityEngine;
using System.Collections.Generic;   

public class WaveGroup 
{
    // 하나의 웨이브 그룹
    public int stageId;
    public int waveIndex;
    public List<WaveData.Data> waveDatas = new List<WaveData.Data>();    
}
