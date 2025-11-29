using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class ConsumableManager : MonoBehaviour
{
    [Header("Referense")]
    [SerializeField] private TowerManager towerManger;
    [SerializeField] private BasePlanet planet;

    [Header("Consumables")]
    [SerializeField] private ConsumableUI consumableUI;
    [SerializeField] private Transform consumableUIRoot;

    private ConsumableFactory consumableFactory = new ConsumableFactory();
    private List<ConsumalbeTable.Data> useAbleConsumList;

    private void Init()
    {
        useAbleConsumList = DataTableManager.ConsumalbeTable.GetDatasWithCondition(towerManger.GetAllTower());

        //임시 테스트용
        var useAbleConsumableKeys = consumableFactory.GetAllKeys();
        var removeList = new List<ConsumalbeTable.Data>();
       
        for(int i = 0;i < useAbleConsumList.Count; i++)
        {
            if (!useAbleConsumableKeys.Contains(useAbleConsumList[i].Item_id))
            {
                removeList.Add(useAbleConsumList[i]);
            }
        }
        
        foreach(var item in removeList)
        {
            useAbleConsumList.Remove(item);
        }
    }

    public ConsumalbeTable.Data GetRandomData()
    {
        if (useAbleConsumList == null) Init();
        int rand = Random.Range(0, useAbleConsumList.Count);
        return useAbleConsumList[rand];
    }
  
    public void SetConsumable(ConsumalbeTable.Data data)
    {
        Consumable consumable = consumableFactory.CreateInstance(data.Item_id);
        consumable.Init(towerManger, planet, data);
        ConsumableUI ui = Instantiate(consumableUI, consumableUIRoot);
        ui.SetConsumable(consumable);
    }
}
