using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitlePresetWindow : Window
{
    [SerializeField] private Button backButton;
    [SerializeField] private PresetViewer presetViewer;
    [SerializeField] private Transform presetDataRoot;
    private List<PresetViewer> presetViewers = new List<PresetViewer>();
    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitlePresetWindow;
        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleStageSelectedWindow));

        DataTableManager.PresetTable.OnChangeDatas += ChangePresetData;
        UpdatePreset();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    private void OnDestroy()
    {
        DataTableManager.PresetTable.OnChangeDatas -= ChangePresetData;
    }

    private void UpdatePreset()
    {
        for(int i = 0; i < presetViewers.Count; i++)
        {
            Destroy(presetViewers[i].gameObject);
        }
        presetViewers.Clear();

        for (int i = 0; i < DataTableManager.PresetTable.Count(); i++)
        {
            var presetViewer = Instantiate(this.presetViewer, presetDataRoot);
            presetViewer.Init(DataTableManager.PresetTable.Get(i), i, manager);
            presetViewers.Add(presetViewer);
        }
    }

    private void ChangePresetData()
    {
        Debug.Log("Preset ChangeData Call");
        int changeIdx = TitleTowerPlaceEditWindow.currentPresetIndex;
        presetViewers[changeIdx].UpdatePreset(DataTableManager.PresetTable.Get(changeIdx));
    }
}
