using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectOptionUI : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image image;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI slotIndexText; 
    [SerializeField] private TextMeshProUGUI descriptionText;

    private RandomOptionBase optionBase;
    private Toggle toggle;
    private Outline outLine;

    private Action<int> OnChangeIndex;

    private Tower tower;

    public void Initalized(int index, Action<int> callback)
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
            }
            else
            {
                outLine.enabled = false;
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
        slotIndexText.text = data.SlotIndex + "¹ø ½½¸©";
        descriptionText.text = optionBase.GetOptionStringFormatting();
    }

    public void ResetOutline()
    {
        outLine.enabled = false;
    }
}
