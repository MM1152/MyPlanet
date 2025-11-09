using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    private Dictionary<int, Data> enemyTable = new Dictionary<int, Data>();
    public class Data
    {
        public string name;
        public float speed;
        public int health;
        public int damage;
        public int attackInterval;
        public ElementType elementType;
    }

    public EnemyData()
    {
        enemyTable.Add(1, new Data()
        {
            name = "xxx",
            speed = 2.0f,
            health = 100,
            damage = 10,
            attackInterval = 1,
            elementType = ElementType.Fire
        });
        enemyTable.Add(2, new Data()
        {
            name = "yyy",
            speed = 1f,
            health = 200,
            damage = 20,
            attackInterval = 2
        });

    }
    public Data GetData(int id)
    {
        if (!enemyTable.ContainsKey(id))
        {
#if DEBUG_MODE
            throw new System.Exception($"EnemyData  ID: {id} is not found.");
#endif
        }
        return enemyTable[id];
    }
}
