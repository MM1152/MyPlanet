using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public JoyStick joyStick;
    public InputAction action;
    private bool isOnUI;

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
