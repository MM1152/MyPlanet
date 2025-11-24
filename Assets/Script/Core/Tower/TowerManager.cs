using UnityEngine;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private Enemy testTarget;
    [SerializeField] private GameObject tower;
    [SerializeField] private EnemySpawnManager enemySpawnManager;
    [SerializeField] private SliderValue expSlider;

    public GameObject basePlanet;

    private List<Tower> towers = new List<Tower>();
    public List<Tower> Towers => towers;
    private TowerFactory towerFactory = new TowerFactory();

    private int totalExp = 0;
    private int currentLevel = 1;
    public int CurrentLevel => currentLevel;
    private int maxLevel = 5;
    public int levelUpExp => currentLevel * 100;

    private bool isLevelUp = false;

    private WindowManager windowManager;
    //Fix : 임시용임
    private PresetData.Data presetGameData;
    public Action<Tower, Collider2D> OnHitTarget;
#if DEBUG_MODE
    [Header("DEBUG")]
    public bool stopAttack;
    public bool disAbleLevelUp;
#endif

    private void Awake()
    {
        windowManager = GameObject.FindGameObjectWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        presetGameData = FirebaseManager.Instance.PresetData.GetGameData();
        for(int i = 0; i < presetGameData.TowerId.Count; i++)
        {
            int towerId = presetGameData.TowerId[i];
            if (towerId == -1)
            {
                AddTower(null, i + 1);
                continue;
            }

            var data = DataTableManager.TowerTable.Get(towerId);
            AddTower(data , i + 1);
        }
    }

    private void Start()
    {
        expSlider.UpdateSlider(0, levelUpExp , currentLevel , levelUpExp - totalExp);
    }

    public void LateUpdate()
    {
#if DEBUG_MODE
        if(stopAttack)
        {
            return;
        }
#endif
        foreach(var tower in towers)
        {
            tower?.Update(Time.deltaTime);
        }
    }

    public List<Enemy> FindTargets()
    {
        return enemySpawnManager.GetEnemyDatas(tower.transform.position);
    }

    public List<Enemy> FindTargets(Vector3 pos)
    {
        return enemySpawnManager.GetEnemyDatas(pos);
    }

    public Enemy FindTarget()
    {
        return enemySpawnManager.GetEnemyData(tower.transform.position);
    }

    public Enemy FindTargetLast()
    {
        var enemys = enemySpawnManager.GetEnemyDatas(tower.transform.position);
        if(enemys != null) 
            return enemys[enemys.Count - 1];

        return null;
    }

    public Enemy FindTargetInRange(Vector3 targetPosition , float distance)
    {
        var targets = FindTargets();
        if(targets == null)
            return null;

        var inRangeTargets = targets.Where(x => Vector3.Distance(x.transform.position, targetPosition) <= distance).ToList();
        if(inRangeTargets.Count == 0)
            return null;
        int rand = UnityEngine.Random.Range(0, inRangeTargets.Count);
        return inRangeTargets[rand];
    }

    public void AddTower(TowerTable.Data data , int slotIndex)
    {
        if(data == null)
        {
            towers.Add(null);
            return;
        }

        Tower instanceTower = towerFactory.CreateInstance(data.ID);
        towers.Add(instanceTower);
        instanceTower.Init(tower, this, data , slotIndex);
    }

    public void PlaceTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);
        var levelUpData = DataTableManager.LevelUpTable.Get(towerData.ID, towers[index].Level + 1);

        if (towers[index].UseAble)
        {
            if(levelUpData != null)
            {
                towers[index].LevelUp(levelUpData);
            }
            return;
        }
        towers[index].LevelUp(levelUpData);
        towers[index].PlaceTower();
    }

    private int FindTowerPlaceIndex(TowerTable.Data towerData)
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == null) continue;
            if (towers[i].ID == towerData.ID)
            {
                return i;
            }
        }
        return -1;
    }

    public List<Tower> GetRightTower(TowerTable.Data towerData, int radius)
    {
        int target = FindTowerPlaceIndex(towerData);
        if (target == -1)
        {
            return null;
        }
        List<Tower> returnTowers = new List<Tower>();
        Tower tower = towers[Utils.ClampIndex(target + radius, towers.Count)];

        if (tower != null)
        {
            returnTowers.Add(tower);
        }
        return returnTowers;
    }

    public List<Tower> GetLeftTower(TowerTable.Data towerData, int radius)
    {
        int target = FindTowerPlaceIndex(towerData);
        if (target == -1)
        {
            return null;
        }
        List<Tower> returnTowers = new List<Tower>();
        Tower tower = towers[Utils.ClampIndex(target - radius, towers.Count)];

        if( tower != null )
        {
            returnTowers.Add(tower);
        }
        return returnTowers;
    }

    public List<Tower> GetAroundTower(TowerTable.Data towerData, int radius)
    {
        int target = FindTowerPlaceIndex(towerData);
        if (target == -1)
        {
            return null;
        }

        List<Tower> targetedTowres = new List<Tower>();

        for (int i = 1; i <= radius; i++)
        {
            int leftPointer = Utils.ClampIndex(target - i, towers.Count);
            int rightPointer = Utils.ClampIndex(target + i, towers.Count);
            Debug.Log($"leftPointer : {leftPointer + 1} , rightPointer : {rightPointer + 1}");
            targetedTowres.Add(towers[leftPointer]);
            targetedTowres.Add(towers[rightPointer]);
        }

        return targetedTowres;
    }

    public Tower GetRandomTower()
    {
        Tower tower = null;
        do
        {
            int rand = UnityEngine.Random.Range(0, towers.Count);
            tower = towers[rand];
        } while (tower == null);

        return tower;
    }

    public void AddExp(int exp)
    {
#if DEBUG_MODE
        if (!disAbleLevelUp) return;
#endif
        var sumExp = Mathf.Min(totalExp += exp, levelUpExp);

#if DEBUG_MODE
        Debug.Log($"Current Exp : {sumExp} / {levelUpExp}");
#endif
        if (sumExp >= levelUpExp)
        {
#if DEBUG_MODE
            Debug.Log("Level Up!");
#endif
            LevelUp();
        }
        expSlider.UpdateSlider(totalExp, levelUpExp, currentLevel, levelUpExp - totalExp);
    }

    private void LevelUp()
    {
        if (currentLevel >= maxLevel)
        {
#if DEBUG_MODE
            Debug.Log("Max Level.");
#endif
            return;
        }
        currentLevel = Mathf.Min(currentLevel + 1, maxLevel);
        totalExp = 0;


        windowManager.Open(WindowIds.PlaceTowerWindow);
    }

    public Tower GetTower(int id)
    {
        return towers[id];
    }

    public Tower GetIdToTower(int id)
    {
        for(int i = 0; i < towers.Count; i++)
        {
            if (towers[i].ID == id)
            {
                return towers[i];
            }
        }

        return null;
    }

    public List<Tower> GetTowerToAttribute(ElementType elementType)
    {
        var elementTower = towers.Where(x => x != null &&  x.GetElementType() == elementType).ToList();
        return elementTower;
    }

    public List<Tower> GetAllTower()
    {
        return towers;
    }
}
