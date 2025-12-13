using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RandomPickUp10Result : MonoBehaviour
{
    [SerializeField] private TenPickUpItemLayout layout;
    [SerializeField] private Transform layoutRoot;
    [SerializeField] private Button closeButton;

    [Header("Ref")]
    [SerializeField] private WindowManager windowManager;
    private List<TenPickUpItemLayout> itemLayouts = new List<TenPickUpItemLayout>();

    public void Awake()
    {
        for(int i = 0; i < 10; i++)
        {
            itemLayouts.Add(Instantiate(layout, layoutRoot));
        }

        closeButton.onClick.AddListener(() => windowManager.Open(WindowIds.RandomPickUpWindow));
    }

    public void SetData(List<RandomPickUpTable.Data> pickUpdatas , List<bool> isNew , List<bool> isDuplication) 
    {
        for (int i = 0; i < pickUpdatas.Count; i++)
        {
            itemLayouts[i].UpdateData(pickUpdatas[i] , isNew[i] , isDuplication[i]);
        }
    }
}
