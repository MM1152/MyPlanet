using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button lobbyButton;

    public JoyStick joyStick;
    public InputAction action;
    private bool isOnUI;

    private void Start()
    {
        lobbyButton.onClick.AddListener(() =>
        {
            LoadingScene.sceneId = SceneIds.TitleScene;
            SceneManager.LoadScene(SceneIds.LoadingScene);
        });
    }

    public void TouchJoyStick(InputAction.CallbackContext context)
    {
        Vector3 inputPos = Touchscreen.current.position.ReadValue();
        bool isOnUI = Utils.IsPointerOverUI(inputPos);
        
        if (joyStick != null && context.started && Variable.IsJoyStickActive && !isOnUI)
        {
            joyStick.ShowJoystick(inputPos);
        }
        else if (context.canceled)
        {
            joyStick.HideJoystick();
        }
    }

}
