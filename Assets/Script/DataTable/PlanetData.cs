using System.Collections.Generic;

public class PlanetData
{
    public class Data
    {
        public int id;
        public string name;
        public int hp;
        public int elementType;
    }

    private Dictionary<int, Data> dataDictionary = new Dictionary<int, Data>();

    public PlanetData()
    {
        Data data1 = new Data() { id = 123, name = "Default Planet" , hp = 100000 , elementType =  (int)ElementType.Fire};    
        dataDictionary.Add(data1.id, data1);
    }

    public Data Get(int id)
    {
        return dataDictionary[id];
    }
}
