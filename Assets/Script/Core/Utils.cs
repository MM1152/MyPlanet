using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    private static Rect screenRect;
    static Utils()
    {
        var camera = Camera.main;
        var zDistance = Mathf.Abs(camera.transform.position.z);

        var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
        var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

        screenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

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

    public static string FormatText(string text, params (string key, string value)[] replacements)
    {
        foreach (var (key, value) in replacements)
        {
            text = text.Replace($"[{key}]", value);
        }
        return text;
    }

    public static Rect GetScreenBounds()
    {
        return screenRect;
    }

    public static void Suffle<T>(IList<T> target)
    {
        int n = target.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = rnd.Next(n--);
            T temp = target[n];
            target[n] = target[k];
            target[k] = temp;
        }
    }
}

