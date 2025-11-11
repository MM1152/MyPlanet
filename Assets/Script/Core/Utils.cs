using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static bool IsPointerOverUI(Vector2 screenPosition)
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results.Count > 0;
    }

    public static int ClampIndex(int index , int count)
    {
        if (index < 0)
            index = index + count;
        else if (index >= count)
            index = index % count;

        return index;
    }
}

