using UnityEngine;
using UnityEngine.UI;

public class PresetViewer : MonoBehaviour
{
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private Button editButton;

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
        }

        editButton.onClick.AddListener(() =>
        {
            TitleTowerPlaceEditWindow.currentPresetIndex = this.index;
            manager.Open(WindowIds.TitleTowerPlaceEditWindow);
        });
    }
}
    