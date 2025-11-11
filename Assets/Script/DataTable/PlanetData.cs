using System.Collections.Generic;

public class PlanetData
{
    public class Data
    {
        public int ID;
        public int Name; 
        public int Explanation;
        public string Rescoce_ID;
        public char Grade; // 등급 표시
        public int Planet_type; // 행성 타입 (1 : 암석 , 2 : 가스 , 3 : 왜소)
        public int Attribute; // 속성 ( 불 , 물 , 빛 , 어둠 , 강철 )
        public int HP; 
        public int ATK;
        public int DEF;
    }

    private Dictionary<int, Data> dataDictionary = new Dictionary<int, Data>();

    public PlanetData()
    {
        Data data1 = new Data() { ID = 123, Name = 1 , HP = 100000 , Planet_type =  (int)ElementType.Fire};    
        dataDictionary.Add(data1.ID, data1);
    }

    public Data Get(int id)
    {
        return dataDictionary[id];
    }
}
