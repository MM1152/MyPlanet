using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlaceTower : MonoBehaviour
{
    [SerializeField] private TowerPlaceHold placeHold;
    // FIX : 추후 행성에 맞춰서 동적으로 변경
    [SerializeField] private int placeHoldCount;
    [SerializeField] private Button rotateLeft;
    [SerializeField] private Button rotateRight;
    [SerializeField] private int currentIndex;

    private RectTransform rect;
    private TowerPlaceHold[] placeHoldList;
    private float angle;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        float radius = rect.rect.width / 2f;
        placeHoldList = new TowerPlaceHold[placeHoldCount];

        angle = 360f / placeHoldCount;

        //각도 나눠서 타워 설치 위치 조정 및
        //설치된 타워 구분용으로 사용됨
        for(int i = 0; i < placeHoldCount; i++)
        {
            TowerPlaceHold obj = Instantiate(placeHold, transform);
            obj.Init(i + 1);
            placeHoldList[i] = obj;

            RectTransform objRect = obj.GetComponent<RectTransform>();

            float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle * i);
            float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle * i);
            objRect.anchoredPosition = new Vector2(y, x);
            objRect.transform.rotation = Quaternion.Euler(0f , 0f , -angle * i);
        }

        rotateLeft.onClick.AddListener(() => Rotate(-1));
        rotateRight.onClick.AddListener(() => Rotate(1));
    }
    

    private void Rotate(float dir)
    {
        gameObject.transform.eulerAngles += new Vector3(0f, 0f, dir * angle);
        currentIndex += (int)dir;
        if (currentIndex == -1)
            currentIndex = placeHoldCount - 1;
        else if(currentIndex >= placeHoldCount)
            currentIndex = currentIndex % placeHoldCount;   
    }

    public bool Place()
    {
        if (placeHoldList[currentIndex].GetPlaced()) return false;

        placeHoldList[currentIndex].SetPlace();
        return true;
    }
}
