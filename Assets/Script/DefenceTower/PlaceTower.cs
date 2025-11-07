using UnityEngine;
using System.Collections.Generic;

public class PlaceTower : MonoBehaviour
{
    [SerializeField] private GameObject placeHold;
    // FIX : 추후 행성에 맞춰서 동적으로 변경
    [SerializeField] private int placeHoldCount;

    private RectTransform rect;
    private int[] placedList;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        float radius = rect.rect.width / 2f;
        placedList = new int[placeHoldCount];

        float angle = 360f / placeHoldCount;
        for(int i = 0; i < placeHoldCount; i++)
        {
            GameObject obj = Instantiate(placeHold, transform);
            RectTransform objRect = obj.GetComponent<RectTransform>();

            float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle * i);
            float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle * i);
            objRect.anchoredPosition = new Vector2(y, x);
            objRect.transform.rotation = Quaternion.Euler(0f , 0f , -angle * i);
        }   
    }
}
