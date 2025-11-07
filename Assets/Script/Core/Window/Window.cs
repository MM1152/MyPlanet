using UnityEngine;

public class Window : MonoBehaviour
{
    protected int windowId;
    public int WindowId => windowId;

    public virtual void Init()
    {
        windowId = (int)WindowIds.None;
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
