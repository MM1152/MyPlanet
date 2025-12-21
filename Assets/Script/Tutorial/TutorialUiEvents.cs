using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialUiEvents : MonoBehaviour, IPointerClickHandler
{
    public event Action OnClickAction;

    private void OnDestroy()
    {
        OnClickAction = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickAction?.Invoke();
    }
}
