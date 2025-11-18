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

    private TowerTable.Data towerData;
    private Toggle toggle;
    private Outline outLine;

    private Action<int> OnChangeIndex;

    public void Initalized(int index , Action<int> callback)
    {
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
        this.towerData = data.TowerData;
        towerNameText.text = data.ID.ToString() + "\n";
        towerTypeText.text = "임시 유형";
        slotIndexText.text = data.SlotIndex + "번 슬릇";
        towerDescriptionText.text = "대충 이런 타워입니다~";
    }

    public TowerTable.Data GetTowerData()
    {
        return towerData;
    }

    public void ResetOutline()
    {
        outLine.enabled = false;
    }
}
