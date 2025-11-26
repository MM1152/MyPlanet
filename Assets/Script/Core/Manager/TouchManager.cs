using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum TouchTypes
{
    None,
    Tab,
    LongTab,
    LongPress,
    Drag,
    NoTab,
}

public class TouchManager : MonoBehaviour
{
    [SerializeField] private TouchTypes touchTypes = TouchTypes.None;

    public TouchTypes TouchType => touchTypes;

    private float longTabTime = 0.5f;
    [SerializeField] private float startTouchTime = 0f;
    [SerializeField] private float notTabToDragDistance = 5f;
    [SerializeField] private float dragDistance = 100f;

    public Vector2 startTouchPosition = Vector2.zero;
    public Vector2 endTouchPosition = Vector2.zero;

    public InputAction touchAction;
    public InputAction touchPositionAction;
    private bool isPressed = false;
    public void Init()
    {
        touchAction = new InputAction
        (
            type: InputActionType.Button,
            binding: "<Touchscreen>/Press"
        );

        touchPositionAction = new InputAction
        (
            type : InputActionType.Value,
            binding : "<Touchscreen>/position"
        );

        touchAction.started += OnTouchStart;
        touchAction.canceled += OnTouchEnd;

        touchPositionAction.performed += OnTouchPosition;
        touchPositionAction.Enable();
        touchAction.Enable();
    }

    private void Update()
    {
        if(isPressed && touchTypes != TouchTypes.NoTab && touchTypes != TouchTypes.Drag)
        {
            if(Time.unscaledTime - startTouchTime > longTabTime)
            {
                touchTypes = TouchTypes.LongPress;
            }
        }
    }

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        endTouchPosition = context.ReadValue<Vector2>();
        var distance = Vector2.Distance(startTouchPosition, endTouchPosition);
        Debug.Log("Distance: " + distance);

        if (distance > notTabToDragDistance)
        {
            touchTypes = TouchTypes.NoTab;
            Debug.Log("NoTab");
            if (distance > dragDistance)
            {
                touchTypes = TouchTypes.Drag;
                Debug.Log("Drag");
            }
        }
    }
    private void OnTouchStart(InputAction.CallbackContext context)
    {
        startTouchTime = Time.unscaledTime;
        isPressed = true;

        startTouchPosition = touchPositionAction.ReadValue<Vector2>();
        endTouchPosition = startTouchPosition;
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        if (touchTypes == TouchTypes.NoTab || touchTypes == TouchTypes.Drag) return;

        Debug.Log("Touch End");

        if (Time.unscaledTime - startTouchTime > longTabTime)
        {
            touchTypes = TouchTypes.LongTab;
        }
        else
        {
            touchTypes = TouchTypes.Tab;
        }
        isPressed = false;
    }

    private void LateUpdate()
    {
        touchTypes = TouchTypes.None;
    }

    private List<RaycastResult> FindUi()
    {
        var result = new List<RaycastResult>();
        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = startTouchPosition
        };

        EventSystem.current.RaycastAll(pointerEventData, result);
        return result;
    }

    public bool OnTargetUI(GameObject targetUI)
    {
        var result = FindUi();

        foreach(var go in result)
        {
            if(go.gameObject == targetUI)
            {
                return true;
            }
        }

        return false;
    }
}