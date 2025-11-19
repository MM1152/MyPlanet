using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class JoyStick : MonoBehaviour
{    
    private CanvasGroup canvasGroup; 
    public OnScreenStick onScreenStickq; 
    public RectTransform pivot;    
    public RectTransform handle;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    private void Start()
    {    
        float range = onScreenStickq.movementRange;
        pivot.sizeDelta = new Vector2(range * 2, range * 2);
     
         HideJoystick();
    }
    
    public void ShowJoystick(Vector3 inputPos)
    {    
        this.transform.position = inputPos;    
        canvasGroup.alpha = 1;    
        canvasGroup.blocksRaycasts = true;
    }
    
    public void HideJoystick()
    {     
        canvasGroup.alpha = 0;     
        canvasGroup.blocksRaycasts = false;        
        handle.transform.position = pivot.position;
    }
}







