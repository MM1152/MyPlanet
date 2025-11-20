using UnityEngine;
using System.Collections.Generic;   

public class StageData 
{
    // 하나의 스테이지 데이터에 인덱스별 웨이브 그룹들  
    public int stageId;
    public List<WaveGroup> waveGroups = new List<WaveGroup>();  
}
