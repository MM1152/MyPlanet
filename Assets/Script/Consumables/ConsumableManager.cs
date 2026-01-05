using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
public class ConsumableManager : MonoBehaviour
{
    private class ActiveConsumable
    {
        public int itemId;
        public Consumable consumable;
        public ConsumableUI ui;
        public float activatedTime; // 활성시점 보려고 
    }

    [Header("Referense")]
    [SerializeField] private TowerManager towerManger;
    [SerializeField] private BasePlanet planet;

    [Header("Consumables")]
    [SerializeField] private ConsumableUI consumableUI;
    [SerializeField] private Transform consumableUIRoot;

    private ConsumableFactory consumableFactory = new ConsumableFactory();
    private List<ConsumalbeTable.Data> useAbleConsumList;
    private List<ActiveConsumable> activeConsumables = new List<ActiveConsumable>();
    private const int maxConsumableCount = 2;

    private bool init = false;
    private void Init()
    {
        useAbleConsumList = DataTableManager.ConsumalbeTable.GetDatasWithCondition(towerManger.GetAllTower());
        init = true;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!init) Init();
            var randomData = GetRandomData();
            SetConsumable(randomData);
            Debug.Log($"[Test] 소모품 추가: {randomData.Name}");
        }
#endif

        for (int i = activeConsumables.Count - 1; i >= 0; i--)
        {
            if (activeConsumables[i].consumable.GetDuration() <= 0f)
            {
                RemoveConsumable(activeConsumables[i]);
            }
        }
    }

    private void UpdateConsume()
    {
        var useAbleConsumableKeys = consumableFactory.GetAllKeys();
        var removeList = new List<ConsumalbeTable.Data>();

        for (int i = 0; i < useAbleConsumList.Count; i++)
        {
            if (!useAbleConsumableKeys.Contains(useAbleConsumList[i].Item_id))
            {
                removeList.Add(useAbleConsumList[i]);
            }
        }

        foreach (var item in removeList)
        {
            useAbleConsumList.Remove(item);
        }
    }

    public ConsumalbeTable.Data GetRandomData()
    {
        int rand = Random.Range(0, useAbleConsumList.Count);
        return useAbleConsumList[rand];
    }

    public ConsumalbeTable.Data GetData(int index)
    {
        return useAbleConsumList[index];
    }
    public List<ConsumalbeTable.Data> GetAllData()
    {
        if (!init) Init();
        return useAbleConsumList.ToList();
    }

    public void SetConsumable(ConsumalbeTable.Data data)
    {
        var con = activeConsumables.Find(x => x.itemId == data.Item_id);
        if (con != null)
        {
            con.consumable.RestDuration();
            return;
        }

        if (activeConsumables.Count >= maxConsumableCount)
        {
            var oldest = activeConsumables.OrderBy(x => x.activatedTime).First();
            RemoveConsumable(oldest);

        }
        UpdateConsume();
        Consumable consumable = consumableFactory.CreateInstance(data.Item_id);
        consumable.Init(towerManger, planet, data);
        ConsumableUI ui = Instantiate(consumableUI, consumableUIRoot);
        ui.SetConsumable(consumable);

        activeConsumables.Add(new ActiveConsumable
        {
            itemId = data.Item_id,
            consumable = consumable,
            ui = ui,
            activatedTime = Time.time
        });
    }

    private void RemoveConsumable(ActiveConsumable activeConsumable)
    {
        activeConsumable.consumable.Release(); 
        activeConsumables.Remove(activeConsumable);
    }

}
