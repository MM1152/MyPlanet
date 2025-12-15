using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectOptionUI : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image backgroundImage;
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
                UpdateBackgroundImage(true);
                OnChangeIndex?.Invoke(index);
            }
            else
            {
                UpdateBackgroundImage(false);
            }
        });
    }

    public Tower GetTowerData()
    {
        return tower;
    }

    public void SetTowerData(Tower data)
    {
        tower = data;
        this.optionBase = data.Option;
        slotIndexText.text = data.SlotIndex + "번 슬롯";
        descriptionText.text = optionBase.GetOptionStringFormatting(); //타워의 각 옵션설명
        
        UpdateBackgroundImage(false);
    }
    
    private void UpdateBackgroundImage(bool isSelected)
    {
        backgroundImage.sprite = isSelected ? selectOptionBackgroundSprite : backgroundSprite;
    }

    public void SetInteractive(bool active)
    {
        toggle.interactable = active;
    }

    // public void ResetOutline()
    // {
    //     tower = null;
    //     optionBase = null;
    //     toggle.isOn = false;
    // }
}
