using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectTowerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerTypeText;
    [SerializeField] private TextMeshProUGUI slotIndexText;
    [SerializeField] private TextMeshProUGUI towerDescriptionText;

    private Image backGroundImage;

    private TowerTable.Data towerData;
    private ConsumalbeTable.Data consumeData;

    private Toggle toggle;
    private Outline outLine;

    private Action<int> OnChangeIndex;

    public void Initalized(int index , Action<int> callback)
    {
        backGroundImage = GetComponent<Image>();
        toggle = GetComponent<Toggle>();
        outLine = GetComponent<Outline>();

        toggle.group = transform.parent.GetComponent<ToggleGroup>();
        OnChangeIndex = callback;

        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                outLine.enabled = true;
                OnChangeIndex?.Invoke(index);
            }else
            {
                outLine.enabled = false;
            }
        });
    }

    public void SetTowerData(Tower data)
    {
        backGroundImage.color = ColorDefine.TowerSelectUIColor;

        this.towerData = data.TowerData;
        towerNameText.text = towerData.Name + (data.Level == 0 ? "" : $"+{data.Level}단계");
        towerTypeText.gameObject.SetActive(true);   
        towerTypeText.text = towerData.AttackType;
        slotIndexText.text = data.SlotIndex + "번 슬릇";
        towerDescriptionText.text = "대충 이런 타워입니다~";
    }

    public void SetConsumableData(ConsumalbeTable.Data data)
    {
        backGroundImage.color = ColorDefine.ConsumableSelectUiColor;

        this.consumeData = data;
        towerNameText.text = data.Name;
        towerTypeText.gameObject.SetActive(false);
        slotIndexText.text = "소모품";
        towerDescriptionText.text = data.Description;
    }

    public TowerTable.Data GetTowerData()
    {
        return towerData;
    }

    public ConsumalbeTable.Data GetCosumaableData()
    {
        return consumeData;
    }

    public void ResetOutline()
    {
        towerData = null;
        consumeData = null;
        outLine.enabled = false;
    }
}
