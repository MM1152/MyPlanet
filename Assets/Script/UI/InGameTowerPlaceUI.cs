using UnityEngine;

public class InGameTowerPlaceUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject checkDragTargetUI;
    [SerializeField] private GameObject moveTargetUI;

    public bool isDrag = false;
    public bool isTargeted = false;

    private void Start()
    {
        moveTargetUI.GetComponent<RectTransform>().sizeDelta = new Vector3(0, canvasRect.rect.height);
    }

    private void Update()
    {
        CheckDragAble();
        if(isTargeted && isDrag)
        {
            moveTargetUI.transform.position = new Vector3(moveTargetUI.transform.position.x, Managers.TouchManager.endTouchPosition.y, 0f);
            Time.timeScale = 0;
        }
    }

    private void CheckDragAble()
    {
        if (Managers.TouchManager.TouchPhase == TouchPhase.Start && Managers.TouchManager.OnTargetUI(checkDragTargetUI))
        {
            isTargeted = true;
        }

        if (isTargeted && !isDrag && Managers.TouchManager.TouchType == TouchTypes.Drag)
        {
            isDrag = true;
            moveTargetUI.SetActive(true);
        }
        else if (Managers.TouchManager.TouchPhase == TouchPhase.End)
        {
            UpdatePanel();
        }
    }

    private void UpdatePanel()
    {
        if(CheckPosition())
        {
            moveTargetUI.GetComponent<RectTransform>().position = new Vector3(moveTargetUI.transform.position.x, 1010);
        }
        else
        {
            moveTargetUI.SetActive(false);
            Time.timeScale = (int)GameSpeed.CurrentSpeed;
        }
        isDrag = false;
        isTargeted = false;
    }

    private bool CheckPosition()
    {
        var point = Camera.main.ScreenToViewportPoint(Managers.TouchManager.endTouchPosition);
        return point.y >= 0.5f;
    }
}
