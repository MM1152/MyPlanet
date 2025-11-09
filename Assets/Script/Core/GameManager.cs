using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public JoyStick joyStick;

    public void TouchJoyStick(InputAction.CallbackContext context)
    {
        Vector3 inputPos = Touchscreen.current.position.ReadValue();
        //조이스틱 보이기 메서드 호출
        if (joyStick != null && context.started)
        {
            joyStick.ShowJoystick(inputPos);
        }
        else if(context.canceled)
        {
            joyStick.HideJoystick();
        }            
    }
}
