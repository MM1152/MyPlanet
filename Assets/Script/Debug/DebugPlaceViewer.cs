using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DebugPlaceViewer : MonoBehaviour
{
    public Tower tower;
    public TextMeshProUGUI towerNameText;
    private Button button;
    public event Action<Tower> callback;

    public void Init(Tower tower)
    {
        this.tower = tower;
        button = GetComponent<Button>();
        UpdateText();

        button.onClick.AddListener(() =>
        {
            callback?.Invoke(tower);
        });
    }

    public void UpdateText()
    {
        towerNameText.text = tower.TowerData.Name;
    }
}
