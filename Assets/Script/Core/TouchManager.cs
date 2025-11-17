using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum TouchTypes
{
    None,
    Tab,
    LongTab,
}

public class TouchManager : MonoBehaviour
{
    [SerializeField] private TouchTypes touchTypes = TouchTypes.None;

    public TouchTypes TouchType => touchTypes;

    private float longTabTime = 0.5f;
    [SerializeField] private float startTouchTime = 0f;

    private Vector2 startTouchPosition = Vector2.zero;

    public InputAction touchAction;
    public InputAction touchPositionAction;
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

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        startTouchPosition = context.ReadValue<Vector2>();
    }

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        startTouchTime = Time.unscaledTime;
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        if(Time.unscaledTime - startTouchTime > longTabTime)
        {
            touchTypes = TouchTypes.LongTab;
        }
        else
        {
            touchTypes = TouchTypes.Tab;
        }
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