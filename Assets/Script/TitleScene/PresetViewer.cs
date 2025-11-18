using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PresetViewer : MonoBehaviour
{
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private Button editButton;
    
    private List<TowerInfomation> towerInfos = new List<TowerInfomation>();
    private PresetTable.Data presetData;
    private int index;
    
    public void Init(PresetTable.Data presetData , int index , WindowManager manager)
    {
        this.presetData = presetData;
        this.index = index;

        for(int i = 0; i < presetData.TowerId.Count; i++)
        {
            if (presetData.TowerId[i] == -1) continue;

            var towerInfo = Instantiate(towerInfomation, towerInfomationRoot);
            towerInfo.Init(presetData.TowerId[i]);
            towerInfos.Add(towerInfo);
        }

        editButton.onClick.AddListener(() =>
        {
            TitleTowerPlaceEditWindow.currentPresetIndex = this.index;
            manager.Open(WindowIds.TitleTowerPlaceEditWindow);
        });
    }

    public void UpdatePreset(PresetTable.Data presetData)
    {
        this.presetData = presetData;

        Debug.Log("Current Update Target : ", gameObject);
        for(int i = 0; i < towerInfos.Count; i++)
        {
            Destroy(towerInfos[i].gameObject);
        }
        towerInfos.Clear();

        for (int i = 0; i < presetData.TowerId.Count; i++)
        {
            if (presetData.TowerId[i] == -1) continue;

            var towerInfo = Instantiate(towerInfomation, towerInfomationRoot);
            towerInfo.Init(presetData.TowerId[i]);
            towerInfos.Add(towerInfo);
        }
    }

}
    