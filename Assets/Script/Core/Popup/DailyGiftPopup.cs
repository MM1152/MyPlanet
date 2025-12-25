using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftPopup : Popup
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI value;
    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.DailyGiftPopup;
    }

    public override void Open()
    {
        base.Open();
    }

    public void SetData(Sprite itemImage , string value) 
    {
        this.itemImage.sprite = itemImage;  
        this.value.text = value;
    }
}