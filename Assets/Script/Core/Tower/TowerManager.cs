using UnityEngine;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private Enemy testTarget;
    [SerializeField] private GameObject tower;
    [SerializeField] private EnemySpawnManager enemySpawnManager;
    [SerializeField] private SliderValue expSlider;

    private List<Tower> towers = new List<Tower>();
    public List<Tower> Towers => towers;
    private TowerFactory towerFactory = new TowerFactory();

    private int totalExp = 0;
    private int currentLevel = 1;
    private int maxLevel = 10;
    public int levelUpExp => currentLevel * 100;

    private bool isLevelUp = false;

    private WindowManager windowManager;
    
#if DEBUG_MODE
    [Header("DEBUG")]
    public bool stopAttack;
#endif

    private async void Awake()
    {
        await DataTableManager.WaitForInitalizeAsync();
        windowManager = GameObject.FindGameObjectWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();
        expSlider.UpdateSlider(0, levelUpExp);
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
            tower.Update(Time.deltaTime);
        }
    }

    public List<Enemy> FindTargets()
    {
        return enemySpawnManager.GetEnemyDatas(tower.transform.position);
    }

    public Enemy FindTarget()
    {
        return enemySpawnManager.GetEnemyData(tower.transform.position);
    }

    public void AddTower(TowerTable.Data data)
    {
        Tower instanceTower = towerFactory.CreateInstance(data.ID);
        towers.Add(instanceTower);
        instanceTower.Init(tower, this, data);
    }

    public void PlaceTower(TowerTable.Data towerData)
    {
        int index = FindTowerPlaceIndex(towerData);

        if (towers[index].UseAble)
        {
            towers[index].LevelUp();
            return;
        }
        towers[index].PlaceTower();
    }

    private int FindTowerPlaceIndex(TowerTable.Data towerData)
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i].ID == towerData.ID)
            {
                return i;
            }
        }
        return -1;
    }

    public List<Tower> GetAroundTower(TowerTable.Data towerData, int radius)
    {
        int target = FindTowerPlaceIndex(towerData);
        if (target == -1)
        {
            return null;
        }

        List<Tower> targetedTowres = new List<Tower>();
#if DEBUG_MODE
        //Debug.Log($"targetIndex : {target} , range : {radius}");
#endif
        for (int i = 1; i <= radius; i++)
        {
            int leftPointer = Utils.ClampIndex(target - i, towers.Count);
            int rightPointer = Utils.ClampIndex(target + i, towers.Count);
            Debug.Log($"leftPointer : {leftPointer} , rightPointer : {rightPointer}");
            targetedTowres.Add(towers[leftPointer]);
            targetedTowres.Add(towers[rightPointer]);
        }

        return targetedTowres;
    }

    public Tower GetRandomTower()
    {
        int rand = UnityEngine.Random.Range(0, towers.Count);
        return towers[rand];
    }

    public void AddExp(int exp)
    {
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
        expSlider.UpdateSlider(sumExp, levelUpExp);
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
}
