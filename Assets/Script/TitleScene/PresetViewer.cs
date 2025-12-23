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
    [SerializeField] private Sprite enableSprite;
    [SerializeField] private Sprite disableSprite;
    [Header("Planet Viewer Reference")]
    [SerializeField] private PlanetInfomation planetInfomation;

    private List<TowerInfomation> towerInfos = new List<TowerInfomation>();
    private PresetData.Data presetData;
    public PresetData.Data PresetData => presetData;
    private Action<int> OnChangeIndex;
    private int index;
    
    public WindowIds CurrentWindowId { get; set; }

    public void Init(PresetData.Data presetData , int index , WindowManager manager , Action<int> OnChangeIndex)
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
                var presetData = FirebaseManager.Instance.PresetData.Get(index);
                planetWindow.SetPresetData(presetData , index);
                if(CurrentWindowId != WindowIds.None)
                {
                    planetWindow.SetPrevWindow(CurrentWindowId);
                }
            }
        });

        selectPresetButton.onClick.AddListener(() =>
        {
            OnClickSelectButton();
        });
    }

    public void UpdatePreset(PresetData.Data presetData)
    {
        this.presetData = presetData;

        for(int i = 0; i < towerInfos.Count; i++)
        {
            Destroy(towerInfos[i].gameObject);
        }

        towerInfos.Clear();

        var planetData = DataTableManager.PlanetTable.Get(presetData.PlanetId);
        planetInfomation.UpdateTexts(planetData);

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
            selectPresetButton.image.sprite = enableSprite;
        }
        else
        {
            selectPresetButton.image.sprite = disableSprite;
        }
    }

    public void OnClickSelectButton()
    {
        OnChangeIndex?.Invoke(index);
    }

    public void DisableEditButton()
    {
        editButton.interactable = false;
    }
}
    