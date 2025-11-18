using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class TitlePresetWindow : Window
{
    [SerializeField] private Button backButton;
    [SerializeField] private PresetViewer presetViewer;
    [SerializeField] private Transform presetDataRoot;
    [SerializeField] private Button gameStartButton;

    private List<PresetViewer> presetViewers = new List<PresetViewer>();
    private int currentSelectPresetIndex = -1;

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitlePresetWindow;
        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleStageSelectedWindow));

        DataTableManager.PresetTable.OnChangePresetData += ChangePresetData;
        UpdatePreset();

        gameStartButton.onClick.AddListener(() =>
        {
            if(currentSelectPresetIndex == -1)
                return;

            LoadingScene.sceneId = SceneIds.GameScene;
            var presetData = presetViewers[currentSelectPresetIndex].PresetData;
            DataTableManager.PresetTable.SetGameData(presetData);
            SceneManager.LoadScene(SceneIds.LoadingScene);
        });
    }

    private void ChangeSelectPresetIndex(int changeIdx)
    {
        if(currentSelectPresetIndex != -1)
        {
            presetViewers[currentSelectPresetIndex].UpdateSelectButton(false);
        }
        currentSelectPresetIndex = changeIdx;
        presetViewers[currentSelectPresetIndex].UpdateSelectButton(true);
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
        DataTableManager.PresetTable.OnChangePresetData -= ChangePresetData;
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
            presetViewer.Init(DataTableManager.PresetTable.Get(i), i, manager , ChangeSelectPresetIndex);
            presetViewers.Add(presetViewer);
        }
    }

    private void ChangePresetData(int index)
    {
        Debug.Log("Preset ChangeData Call");
        var presetData = DataTableManager.PresetTable.Get(index);
        presetViewers[index].UpdatePreset(presetData);
    }
}
