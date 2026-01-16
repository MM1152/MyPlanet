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
    private int currentSelectPresetIndex = 0;

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
            var presetName = presetViewers[currentSelectPresetIndex].PresetName;    
            LoadingScene.sceneId = SceneIds.GameScene;
            FirebaseManager.Instance.PresetData.SetGameData(presetData);
            var popup = popupManger.Open<PlayGamePopup>(PopupIds.PlayGamePopup);
            popup.UpdatePresetData(FirebaseManager.Instance.PresetData.GetGameData(), presetName);
        });
    }

    private void ChangeSelectPresetIndex(int changeIdx)
    {
        if (currentSelectPresetIndex != -1 && currentSelectPresetIndex < presetViewers.Count)
        {
            if (presetViewers[currentSelectPresetIndex] != null)
            {
                presetViewers[currentSelectPresetIndex].UpdateSelectButton(false);
            }
        }
        currentSelectPresetIndex = changeIdx;
        if (currentSelectPresetIndex < presetViewers.Count && presetViewers[currentSelectPresetIndex] != null)
        {
            presetViewers[currentSelectPresetIndex].UpdateSelectButton(true);
        }
    }

    public override void Open()
    {
        if(FirebaseManager.Instance.PresetData.GetGameData().stageId == 1)
        {
            PresetData.Data tempData = new PresetData.Data();
            tempData.TowerId = new List<int>() { 2003 , 2015 , -1 , -1 ,-1 , -1 , -1 , -1 , -1 , -1 , -1 , -1};
            tempData.PlanetId = 1001;

            if (presetViewers.Count > 0 && presetViewers[0] != null)
            {
                presetViewers[0].UpdatePreset(tempData);
                presetViewers[0].UpdateSelectButton(true);
                presetViewers[0].OnClickSelectButton();
                presetViewers[0].DisableEditButton();
            }

            for (int i = 1; i < presetViewers.Count; i++)
            {
                if (presetViewers[i] != null && presetViewers[i].gameObject != null)
                {
                    Destroy(presetViewers[i].gameObject);
                }
                presetViewers.RemoveAt(i--);
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
        if (FirebaseManager.Instance != null && FirebaseManager.Instance.PresetData != null)
        {
            FirebaseManager.Instance.PresetData.OnChangePresetData -= ChangePresetData;
        }
        
        // 남아있는 프리셋 뷰어들 정리
        foreach (var viewer in presetViewers)
        {
            if (viewer != null && viewer.gameObject != null)
            {
                Destroy(viewer.gameObject);
            }
        }
        presetViewers.Clear();
    }

    private void UpdatePreset()
    {
        // 기존 프리셋 뷰어들을 안전하게 제거
        for(int i = 0; i < presetViewers.Count; i++)
        {
            if (presetViewers[i] != null && presetViewers[i].gameObject != null)
            {
                Destroy(presetViewers[i].gameObject);
            }
        }
        presetViewers.Clear();

        for (int i = 0; i < FirebaseManager.Instance.PresetData.Count(); i++)
        {
            var presetViewer = Instantiate(this.presetViewer, presetDataRoot);
            presetViewer.Init(FirebaseManager.Instance.PresetData.Get(i), i, manager , ChangeSelectPresetIndex);
            presetViewers.Add(presetViewer);
            presetViewer.CurrentWindowId = (WindowIds)windowId;
        }
        presetViewers[currentSelectPresetIndex].UpdateSelectButton(true);
    }

    private void ChangePresetData(int index)
    {
        Debug.Log("Preset ChangeData Call");
        
        // 인덱스 유효성 및 null 체크
        if (index >= 0 && index < presetViewers.Count && presetViewers[index] != null)
        {
            var presetData = FirebaseManager.Instance.PresetData.Get(index);
            presetViewers[index].UpdatePreset(presetData);
        }
        else
        {
            Debug.LogWarning($"ChangePresetData: Invalid index {index} or viewer is null");
        }
    }
    
    public Button GetEditButtonForTutorial()
    {
        return presetViewers[0].GetEditButton();
    }

    public void UpdateTutorialPart()
    {
        for(int i = 1; i < presetViewers.Count; i++)
        {
            presetViewers[i].DisableEditButton();
        }
    }

    public void ReseteTutorialPart()
    {
        for (int i = 1; i < presetViewers.Count; i++)
        {
            presetViewers[i].OnEnableEditButton();
        }
    }
}
