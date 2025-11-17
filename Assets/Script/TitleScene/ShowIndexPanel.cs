using System;
using TMPro;
using UnityEngine;

public class ShowIndexPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private int index = -1;
    public int Index => index;

    public event Action<int> OnTab;

    private void OnDestroy()
    {
        OnTab = null;
    }

    public void UpdatePlace(int index)
    {
        if(index == -1)
        {
            gameObject.SetActive(false);
            this.index = -1;
        }
        else
        {
            text.text = index.ToString();
            gameObject.SetActive(true);
            this.index = index;
        }
    }

    private void Update()
    {
        if(index != -1 && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(this.gameObject))
        {
            OnTab?.Invoke(index - 1);   
        }   
    }
}
