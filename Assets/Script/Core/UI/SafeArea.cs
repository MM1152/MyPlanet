using UnityEngine;

public class SafeAreaManager : MonoBehaviour
{
    private RectTransform rect;
    public static Vector4 percentOffset;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ApplySafeArea();
        Debug.Log(percentOffset);
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        percentOffset.x = anchorMin.x / Screen.width;
        percentOffset.y = anchorMin.y / Screen.height;

        percentOffset.w = anchorMax.x / Screen.width;
        percentOffset.z = anchorMax.y / Screen.height;

        rect.anchorMin = new Vector2(percentOffset.x , percentOffset.y);
        rect.anchorMax = new Vector2(percentOffset.w, percentOffset.z);
    }

    public static void ReplacePositionGameObject(GameObject replaceObject)
    {
        replaceObject.transform.position = new Vector3(
            replaceObject.transform.position.x * (1 - percentOffset.x),
            replaceObject.transform.position.y * (1 - percentOffset.y),
            replaceObject.transform.position.z
        );

        replaceObject.transform.position = new Vector3(
            replaceObject.transform.position.x * (percentOffset.w),
            replaceObject.transform.position.y * (percentOffset.z),
            replaceObject.transform.position.z
        );
    }

    public static void ReplaceScaleGameObject(GameObject replaceObject)
    {
        replaceObject.transform.localScale = new Vector3(
            replaceObject.transform.localScale.x * (1 - percentOffset.x),
            replaceObject.transform.localScale.y * (1 - percentOffset.y),
            replaceObject.transform.localScale.z
        );

        replaceObject.transform.localScale = new Vector3(
            replaceObject.transform.localScale.x * (percentOffset.w),
            replaceObject.transform.localScale.y * (percentOffset.z),
            replaceObject.transform.localScale.z
        );
    }
}
