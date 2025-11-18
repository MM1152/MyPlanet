using System;
using TMPro;
using UnityEngine;

public class ShowIndexPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public int index = -1;
    public int Index => index;

    public event Action<int> OnTab;
    private TowerInfomation towerInfoMation;
    
    public void Init(TowerInfomation towerInfoMation)
    {
        this.towerInfoMation = towerInfoMation;
    }

    public void UpdatePlace(int index)
    {
        if(index == -1)
        {
            gameObject.SetActive(false);
            this.index = -1;
            towerInfoMation.DisableTouch = false;
        }
        else
        {
            text.text = index.ToString();
            gameObject.SetActive(true);
            this.index = index;
            towerInfoMation.DisableTouch = true;
        }
    }

    private void Update()
    {
        if(Index != -1 && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(this.gameObject))
        {
            OnTab?.Invoke(Index - 1);   
        }
    }
}
