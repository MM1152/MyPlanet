using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class TitleStageSelectWindow : Window
{
    [SerializeField] private Button backButton;    
    [SerializeField] private Button selectButton;

    [SerializeField] private StageLayout stageLayout;
    [SerializeField] private Transform stageLayoutRoot;

    private List<StageLayout> stageLayouts = new List<StageLayout>();
    private StageLayout currentStageLayout;
    private int currentSelectStage = 0;
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleStageSelectedWindow;

        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));
        selectButton.onClick.AddListener(() => {
            if(currentStageLayout != null)
            {
                FirebaseManager.Instance.PresetData.SetGameDataStageId(currentStageLayout.StageIdx);
                if(currentStageLayout.StageIdx == 1)
                {
                    FirebaseManager.Instance.PresetData.SetGameData(null);
                    LoadingScene.sceneId = SceneIds.GameScene;
                    SceneManager.LoadScene(SceneIds.LoadingScene);
                    return;
                }
                manager.Open(WindowIds.TitlePresetWindow);
            }
        });

        int stageCount = DataTableManager.WaveTable.GetStageCount();
        for(int i = 0; i < stageCount; i++)
        {
            var stage = Instantiate(stageLayout, stageLayoutRoot);
            stage.Init(i + 1 , UpdateStageLayout);
            stageLayouts.Add(stage);
            stage.UpdateStageLayout(false, false, false);
        }
    }

    public void UpdateStageLayout(int idx)
    {
        if (currentStageLayout != null)
            currentStageLayout.UpdateStageLayout(false, false, false);
        
        //FIX : TutorialSecne 막아놓음
        if(idx == 1)
        {
            selectButton.interactable = false;
        }
        else
        {
            selectButton.interactable = true;
        }

            currentSelectStage = idx - 1;
        currentStageLayout = stageLayouts[currentSelectStage];
        bool activeLeftArrow = currentSelectStage > 0;  
        bool activeRightArrow = currentSelectStage < stageLayouts.Count - 1;
        currentStageLayout.UpdateStageLayout(activeLeftArrow, activeRightArrow, true);
    }

    public override void Open()
    {
        if(currentStageLayout != null)
            currentStageLayout.UpdateStageLayout(false, false, false);
        UpdateStageLayout(1);
        base.Open();
    }
}
