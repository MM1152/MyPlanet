using UnityEngine;

public class Window : MonoBehaviour
{
    protected int windowId;
    protected WindowManager manager;
    public int WindowId => windowId;

    public virtual void Init(WindowManager manager)
    {
        this.manager = manager;
        windowId = (int)WindowIds.None;
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        Time.timeScale = 1f; // 게임 재개
        gameObject.SetActive(false);
    }
}
