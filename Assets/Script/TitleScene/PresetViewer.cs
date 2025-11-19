using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;

public class PresetViewer : MonoBehaviour
{
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private Button editButton;
    [SerializeField] private Button selectPresetButton;
    
    [Header("Planet Viewer Reference")]
    [SerializeField] private TextMeshProUGUI planetgradeText;
    [SerializeField] private TextMeshProUGUI planetTypeText;
    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private Image planetImage;

    private List<TowerInfomation> towerInfos = new List<TowerInfomation>();
    private PresetTable.Data presetData;
    public PresetTable.Data PresetData => presetData;
    private Action<int> OnChangeIndex;
    private int index;
    
    public void Init(PresetTable.Data presetData , int index , WindowManager manager , Action<int> OnChangeIndex)
    {
        this.presetData = presetData;
        this.index = index;
        this.OnChangeIndex = OnChangeIndex;
        UpdatePreset(presetData);

        editButton.onClick.AddListener(() =>
        {
            var window = manager.Open(WindowIds.TitleSelectPlanetWindow);
            if(window is TitleSelectPlanetWindow planetWindow)
            {
                var presetData = DataTableManager.PresetTable.Get(index);
                planetWindow.SetPresetData(presetData , index);
            }
        });

        selectPresetButton.onClick.AddListener(() =>
        {
            OnChangeIndex?.Invoke(index);
        });
    }

    public void UpdatePreset(PresetTable.Data presetData)
    {
        this.presetData = presetData;

        for(int i = 0; i < towerInfos.Count; i++)
        {
            Destroy(towerInfos[i].gameObject);
        }

        towerInfos.Clear();

        var planetData = DataTableManager.PlanetTable.Get(presetData.PlanetId);
        if(planetData != null)
        {
            planetgradeText.text = planetData.grade;
            planetNameText.text = planetData.Name;
            planetTypeText.text = planetData.PlanetType;
        }

        for (int i = 0; i < presetData.TowerId.Count; i++)
        {
            if (presetData.TowerId[i] == -1) continue;

            var towerInfo = Instantiate(towerInfomation, towerInfomationRoot);
            towerInfo.Init(presetData.TowerId[i]);
            towerInfos.Add(towerInfo);
        }
    }

    public void UpdateSelectButton(bool active)
    {
        if(active)
        {
            var image = selectPresetButton.GetComponent<Image>();
            if(image != null)
            {
                image.color = Color.yellow;
            }
        }
        else
        {
            var image = selectPresetButton.GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.white;
            }
        }
    }
}
    