using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;

public class PopupManager : MonoBehaviour
{
    public List<Popup> popups = new List<Popup>();
    public GameObject popupBackGroundPanel;
    public GameObject clickToCloseUI;
    private Dictionary<int, Popup> popupTable = new Dictionary<int, Popup>();

    private Stack<Popup> popupStack = new Stack<Popup>();

    private bool interactable = true;
    public bool Interactable => interactable;
    private void Awake()
    {
        foreach (var popup in popups)
        {
            popup.Init(this);
            popupTable.Add(popup.PoopupId, popup);
            popup.ForcingClose();
        }
        UpdateBackGroundPanel().Forget();
    }

    public T Open<T>(PopupIds id) where T : Popup
    {
        WaitForPushEndFrameAsync(popupTable[(int)id]).Forget();
        popupTable[(int)id].Open();
        UpdateBackGroundPanel().Forget();
        interactable = false;
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
        if(Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(clickToCloseUI) && closePopup.Close())
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
        UpdateBackGroundPanel().Forget();
    }

    public void ForceClose()
    {
        var popup = popupStack.Pop();
        popup.Close();
        UpdateBackGroundPanel().Forget();
    }

    private async UniTaskVoid UpdateBackGroundPanel()
    {
        await UniTask.Yield();

        if (popupStack.Count > 0)
        {
            popupBackGroundPanel.SetActive(true);
        }
        else
        {
            popupBackGroundPanel.SetActive(false);
            interactable = true;
        }

        
    }
}