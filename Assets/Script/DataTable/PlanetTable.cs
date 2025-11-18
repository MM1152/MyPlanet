using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlanetTable : DataTable
{
    public class Data
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public int Explantion { get; set; }
        public string Resource_ID { get; set; }
        public char Grade { get; set; }
        public int Planet_type { get; set; }
        public int Attribute { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
    }
    public override UniTask<(string, DataTable)> LoadAsync(string filename)
    {
        throw new System.NotImplementedException();
    }
}
