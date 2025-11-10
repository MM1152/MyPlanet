using System.Collections.Generic;
public class TowerData
{
    private Dictionary<int, Data> towerTable = new Dictionary<int, Data>();

    public class Data
    {
        public string name;
        public int id;
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
            name = "HellFire Gun Tower",
            attackRadius = 5.0f,
            id = 1,
            attackInterval = 0.5f,
            type = (int)ElementType.Fire,
            projectilePrefabPath = "Bullet",
            damage = 10,
            tower = new HellFireGunTower(),
        });
        towerTable.Add(2, new Data()
        {
            name = "Missile Tower",
            attackRadius = 7.0f,
            id = 2,
            attackInterval = 1.5f,
            type = (int)ElementType.Water,
            projectilePrefabPath = "Missile",
            damage = 10,
            tower = new VolcanoLauncher()
        });
        towerTable.Add(3, new Data()
        {
            name = "Laser Tower",
            attackRadius = 6.0f,
            id = 3,
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
