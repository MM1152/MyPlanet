using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectOptionUI : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image iconBgImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImageLeft;
    [SerializeField] private Image iconImageRight;
    [Header("Sprites")]
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private Sprite selectOptionBackgroundSprite;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI slotIndexText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Toggle")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private ToggleGroup toggleGroup;

    private RandomOptionBase optionBase;
    private Action<int> OnChangeIndex;
    private Tower tower;
    private RandomOptionData.Data newRandomOption;
    private int bonusAmount;

    public void Initalized(int index, Action<int> callback)
    {
        if (toggleGroup != null)
        {
            toggle.group = toggleGroup;
        }

        OnChangeIndex = callback;

        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                Managers.SoundManager.PlaySFX(AudiosId.ui_button_simple_click_06);
                UpdateBackgroundImage(true);
                OnChangeIndex?.Invoke(index);
            }
            else
            {
                UpdateBackgroundImage(false);
                OnChangeIndex?.Invoke(-1); 
            }
        });
    }

    public Tower GetTowerData()
    {
        return tower;
    }

    public void SetTowerData(Tower data,Sprite iconSpriteLeft,Sprite iconSpriteRight)
    {
        tower = data;
        
        // slotIndexText.text = data.SlotIndex + "번 슬롯";
        iconImageLeft.sprite = iconSpriteLeft;
        iconImageRight.sprite = iconSpriteRight;
        
        newRandomOption = RandomOptionData.GetRandomOption();

        if(newRandomOption.id == 1)
        {
            bonusAmount = DataTableManager.OptionTable.GetValueDataToInt(5014);
        }
        else if (newRandomOption.id == 2)
        {
            bonusAmount = DataTableManager.OptionTable.GetValueDataToInt(5015);
        }
        else
        {
            bonusAmount = DataTableManager.OptionTable.GetValueDataToInt(5016);
        }
        var str= newRandomOption.option.GetOptionStringFormatting();

        slotIndexText.text = $"{str.Substring(0, str.Length - 2)} 강화";
        descriptionText.text = $"[{newRandomOption.option.GetOptionStringFormatting()}] + {bonusAmount}%";
        
        UpdateBackgroundImage(false);
    }
    
    public RandomOptionData.Data GetNewRandomOption()
    {
        return newRandomOption;
    }
    
    public int GetBonusAmount()
    {
        return bonusAmount;
    }
    
    private void UpdateBackgroundImage(bool isSelected)
    {
        backgroundImage.sprite = isSelected ? selectOptionBackgroundSprite : backgroundSprite;
    }

    public void SetInteractive(bool active)
    {
        toggle.interactable = active;
    }

    public void ResetOutline()
    {
        tower = null;
        optionBase = null;
        toggle.isOn = false;
    }
}
