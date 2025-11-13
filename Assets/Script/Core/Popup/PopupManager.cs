using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private List<Popup> popups = new List<Popup>();
    [SerializeField] private GameObject popupBackGroundPanel;
    private Dictionary<int, Popup> popupTable = new Dictionary<int, Popup>();

    private Stack<Popup> popupStack = new Stack<Popup>();

    private void Awake()
    {
        foreach (var popup in popups)
        {
            popupTable.Add(popup.PoopupId, popup);
            popup.Init(this);
            popup.ForcingClose();
        }
        UpdateBackGroundPanel();
    }

    public T Open<T>(PopupIds id) where T : Popup
    {
        WaitForPushEndFrameAsync(popupTable[(int)id]).Forget();
        popupTable[(int)id].Open();
        UpdateBackGroundPanel();
        return popupTable[(int)id] as T;
    }

    private async UniTaskVoid WaitForPushEndFrameAsync(Popup popup)
    {
        await UniTask.WaitForEndOfFrame();
        popupStack.Push(popup);
    }

    private void Update()
    {
        Close();
    }

    public void Close()
    {
        if (popupStack.Count == 0) return;

        var closePopup = popupStack.Peek();
        if(Managers.TouchManager.TouchType == TouchTypes.Tab && !Managers.TouchManager.OnTargetUI(closePopup.gameObject) && closePopup.Close())
        {
            popupStack.Pop();
        }

        if (popupStack.Count > 0)
        {
            Variable.IsJoyStickActive = false;
        }
        else
        {
            Variable.IsJoyStickActive = true;
        }
        UpdateBackGroundPanel();
    }

    private void UpdateBackGroundPanel()
    {

        if (popupStack.Count > 0)
        {
            popupBackGroundPanel.SetActive(true);
        }
        else
        {
            popupBackGroundPanel.SetActive(false);
        }
    }
}