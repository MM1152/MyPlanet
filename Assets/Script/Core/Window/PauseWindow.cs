using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button continuingButton;


    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.PauseWindow;

        exitButton.onClick.AddListener(OnClickExitButton);
        replayButton.onClick.AddListener(OnClickReplayButton);
        continuingButton.onClick.AddListener(OnClickContinuingButton);
    }
    public override void Open()
    {
        Time.timeScale = 0f;
        base.Open();
    }

    private void OnClickExitButton()
    {
        LoadingScene.sceneId = SceneIds.TitleScene;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }

    private void OnClickReplayButton()
    {
        LoadingScene.sceneId = SceneIds.GameScene;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }

    private void OnClickContinuingButton()
    {
        manager.Close();
    }

    public override void Close()
    {
        base.Close();
    }
}