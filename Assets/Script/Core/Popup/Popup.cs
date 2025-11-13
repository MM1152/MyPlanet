using JetBrains.Annotations;
using UnityEngine;

public class Popup : MonoBehaviour
{
    protected int popupId;
    public int PoopupId => popupId;

    protected PopupManager manager;

    public virtual void Init(PopupManager manager)
    {
        popupId = (int) PopupIds.None;
        this.manager = manager;    
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual bool Close()
    {
        gameObject.SetActive(false);
        return true;
    }

    public void ForcingClose()
    {
        gameObject.SetActive(false);
    }
}