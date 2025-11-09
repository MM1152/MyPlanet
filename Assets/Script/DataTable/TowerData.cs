using System.Collections.Generic;
public class TowerData
{
    private Dictionary<int, Data> towerTable = new Dictionary<int, Data>();

    public class Data
    {
        public string name;
        public float attackRadius;
        public float attackInterval;
        public int type;
        public string projectilePrefabPath;
        public int damage;

        [CsvHelper.Configuration.Attributes.Ignore]
        public Tower tower;
    }

    public TowerData()
    {
        // 임시 데이터 생성
        towerTable.Add(1, new Data()
        {
            name = "Machine Gun Tower",
            attackRadius = 5.0f,
            attackInterval = 0.5f,
            type = (int)ElementType.Fire,
            projectilePrefabPath = "Bullet",
            damage = 10,
            tower = new MuchineGunTower(),
        });
        towerTable.Add(2, new Data()
        {
            name = "Missile Tower",
            attackRadius = 7.0f,
            attackInterval = 1.5f,
            type = (int)ElementType.Water,
            projectilePrefabPath = "Missile",
            damage = 10,
            tower = new MissileTower()
        });
        towerTable.Add(3, new Data()
        {
            name = "Laser Tower",
            attackRadius = 6.0f,
            attackInterval = 2.0f,
            type = (int)ElementType.Steel,
            projectilePrefabPath = "Laser",
            damage = 10,
            tower = new LaserTower()
        });
    }

    public Data GetData(int id)
    {
        if(!towerTable.ContainsKey(id))
        {
#if DEBUG_MODE
            throw new System.Exception($"TowerData 에는 ID {id} 가 존재하지 않습니다.");
#endif
            return null;
        }

        return towerTable[id];
    }
}
