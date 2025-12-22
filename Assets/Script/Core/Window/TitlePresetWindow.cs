using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class TitlePresetWindow : Window
{
    [SerializeField] private Button backButton;
    [SerializeField] private PresetViewer presetViewer;
    [SerializeField] private Transform presetDataRoot;
    [SerializeField] private Button gameStartButton;
    [SerializeField] private PopupManager popupManger;

    public Button GameStartButton => gameStartButton;

    private List<PresetViewer> presetViewers = new List<PresetViewer>();
    private int currentSelectPresetIndex = -1;

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitlePresetWindow;
        backButton.onClick.AddListener(() => {
            manager.Open(WindowIds.TitleStageSelectedWindow);
        });

        FirebaseManager.Instance.PresetData.OnChangePresetData += ChangePresetData;
        UpdatePreset();

        gameStartButton.onClick.AddListener(() =>
        {
            if(currentSelectPresetIndex == -1)
                return;
            var presetData = presetViewers[currentSelectPresetIndex].PresetData;
            var placeTowerCount = 0;

            for(int i = 0; i < presetData.TowerId.Count; i++)
            {
                if(presetData.TowerId[i] != -1)
                {
                    placeTowerCount++;
                }
            }

            if(placeTowerCount == 0)
            {
                return;
            }

            LoadingScene.sceneId = SceneIds.GameScene;
            FirebaseManager.Instance.PresetData.SetGameData(presetData);
            var popup = popupManger.Open<PlayGamePopup>(PopupIds.PlayGamePopup);
            popup.UpdatePresetData(FirebaseManager.Instance.PresetData.GetGameData());
        });
    }

    private void ChangeSelectPresetIndex(int changeIdx)
    {
        if (currentSelectPresetIndex != -1)
        {
            presetViewers[currentSelectPresetIndex].UpdateSelectButton(false);
        }
        currentSelectPresetIndex = changeIdx;
        presetViewers[currentSelectPresetIndex].UpdateSelectButton(true);
    }

    public override void Open()
    {
        if(FirebaseManager.Instance.PresetData.GetGameData().stageId == 1)
        {
            PresetData.Data tempData = new PresetData.Data();
            tempData.TowerId = new List<int>() { 2003 , 2015 , -1 , -1 ,-1 , -1 , -1 , -1 , -1 , -1 , -1 , -1};
            tempData.PlanetId = 1001;

            presetViewers[0].UpdatePreset(tempData);
            presetViewers[0].UpdateSelectButton(true);
            presetViewers[0].OnClickSelectButton();

            for(int i = 1; i < presetViewers.Count; i++)
            {
                Destroy(presetViewers[i].gameObject);
            }
        }
        base.Open();
    }

    public override void Close()
    {
        if(FirebaseManager.Instance.PresetData.GetGameData().stageId == 1)
        {
            UpdatePreset();
        }
        base.Close();
    }

    private void OnDestroy()
    {
        FirebaseManager.Instance.PresetData.OnChangePresetData -= ChangePresetData;
    }

    private void UpdatePreset()
    {

        for(int i = 0; i < presetViewers.Count; i++)
        {
            Destroy(presetViewers[i].gameObject);
        }
        presetViewers.Clear();

        for (int i = 0; i < FirebaseManager.Instance.PresetData.Count(); i++)
        {
            var presetViewer = Instantiate(this.presetViewer, presetDataRoot);
            presetViewer.Init(FirebaseManager.Instance.PresetData.Get(i), i, manager , ChangeSelectPresetIndex);
            presetViewers.Add(presetViewer);
            presetViewer.CurrentWindowId = (WindowIds)windowId;
        }
    }

    private void ChangePresetData(int index)
    {
        Debug.Log("Preset ChangeData Call");
        var presetData = FirebaseManager.Instance.PresetData.Get(index);
        presetViewers[index].UpdatePreset(presetData);
    }
}
