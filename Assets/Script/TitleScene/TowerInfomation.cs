using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfomation : MonoBehaviour
{
    [SerializeField] private Image typeImage;
    [SerializeField] private Image effectiveImage;
    [SerializeField] private TextMeshProUGUI towerTypeText;
    [SerializeField] private TextMeshProUGUI towerNameText;
    private TowerTable.Data data;

    public event Action<TowerTable.Data> OnTab;
    public bool DisableTouch { get; set; } = false;
    public void Init(int towerId)
    {
        data = DataTableManager.TowerTable.Get(towerId);
        towerNameText.text = data.Name;
    }

    public TowerTable.Data GetTowerData()
    {
        return data;
    }

    private void Update()
    {
        if(!DisableTouch && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(this.gameObject))
        {
            OnTab?.Invoke(data);
        }
    }
}
