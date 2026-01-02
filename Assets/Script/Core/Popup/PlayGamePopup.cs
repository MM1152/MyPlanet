using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGamePopup : Popup
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI presetText;

    [Header("Buttons")]
    [SerializeField] private Button gamePlayButton;
    [SerializeField] private Button cancelButton;

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.PlayGamePopup;

        cancelButton.onClick.AddListener(() =>
        {
            manager.ForceClose();
        });
        gamePlayButton.onClick.AddListener(() =>
        {
            LoadingScene.sceneId = SceneIds.GameScene;
            SceneManager.LoadScene(SceneIds.LoadingScene);
        });
    }

    public override void Open()
    {
        base.Open();
    }

    public void UpdatePresetData(PresetData.InGameData presetData,string presetName)
    {
        stageText.text = $"{presetData.stageId:D3}";
        //  presetText.text = presetData.data.PresetName;
        presetText.text = presetName;
    }
}
