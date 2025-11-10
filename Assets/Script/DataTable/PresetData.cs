using System.Collections.Generic;

public class PresetData
{
    public List<int> towerIds;

    public PresetData(List<int> towerIds)
    {
        this.towerIds = new List<int>();

        //FIX : 추후에 프리셋 데이터로 변경
        this.towerIds.Add(1);
        this.towerIds.Add(2);
        this.towerIds.Add(3);
        this.towerIds.Add(4);
        this.towerIds.Add(5);
        this.towerIds.Add(6);
        this.towerIds.Add(7);
        this.towerIds.Add(8);
    }
}

