using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugScene : MonoBehaviour
{
    [Header ("Buttons")]
    public Button backButton;

    [Header ("References")]
    public WindowManager windowManager;

    public void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            LoadingScene.sceneId = SceneIds.TitleScene;
            SceneManager.LoadScene(SceneIds.LoadingScene);
        });
    }
}
