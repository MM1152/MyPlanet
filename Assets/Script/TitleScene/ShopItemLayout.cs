using System;
using TMPro;
using UnityEngine;

public class ShopItemLayout : MonoBehaviour
{
    [SerializeField] private TowerInfomation towerinfomation;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject disablePanel;

    public event Action<ShopTable.Data> OnClick;
    private ShopTable.Data itemData;
    private bool isDisabled = false;
    public void Init(ShopTable.Data itemData)
    {
        this.itemData = itemData;
        towerinfomation.Init(itemData.Tower_ID);
        priceText.text = itemData.Price.ToString();
    }

    private void Update()
    {
        if(!isDisabled && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(this.gameObject)) 
        {
            OnClick?.Invoke(itemData);
        }
    }

    public void Disable()
    {
        disablePanel.SetActive(true);
        priceText.color = Color.red;
        isDisabled = true;
    }

    public void Enable()
    {
        disablePanel.SetActive(false);
        priceText.color = Color.white;
        isDisabled = false;
    }

    public int GetPrice()
    {
        return itemData.Price;
    }   

    public int GetTowerID()
    {
        return itemData.Tower_ID;
    }

    private void OnDestroy()
    {
        OnClick = null;
    }
}
