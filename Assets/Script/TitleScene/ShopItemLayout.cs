using System;
using TMPro;
using UnityEngine;

public class ShopItemLayout : MonoBehaviour
{
    [SerializeField] private TowerInfomation towerinfomation;
    [SerializeField] private TextMeshProUGUI priceText;

    public event Action<ShopTable.Data> OnClick;
    private ShopTable.Data itemData;

    public void Init(ShopTable.Data itemData)
    {
        this.itemData = itemData;
        towerinfomation.Init(itemData.Tower_ID);
        priceText.text = itemData.Price.ToString();
    }

    private void Update()
    {
        if(Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(this.gameObject)) 
        {
            OnClick?.Invoke(itemData);
        }
    }

    public void Disable()
    {
        //FIX : 추후에 막기 기능 추가
    }

    private void OnDestroy()
    {
        OnClick = null;
    }
}
