using UnityEngine;

public class  Window : MonoBehaviour
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
        gameObject.SetActive(false);
    }

    public virtual void TutorialTowerOpen1()
    {
        gameObject.SetActive(true);
    }
    public virtual void TutorialTowerOpen2()
    {
        gameObject.SetActive(true);
    }
    public virtual void TutorialTowerOpen3()
    {
        gameObject.SetActive(true);
    }
}
