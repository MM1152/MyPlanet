using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPickUpResult : MonoBehaviour
{
    [SerializeField] private TowerItemViewer towerItemViewer;
    [SerializeField] private Transform towerPickUpViewerRoot;
    [SerializeField] private Button okButton;
    [SerializeField] private WindowManager windowManager;

    public Button OkButton => okButton;
    private List<TowerItemViewer> towerItemViewers = new List<TowerItemViewer>();

    public void Awake()
    {
        for(int i = 0; i < 10; i++)
        {
            towerItemViewers.Add(Instantiate(towerItemViewer, towerPickUpViewerRoot));
        }

        okButton.onClick.AddListener(() => windowManager.Open(WindowIds.RandomPickUpWindow));
    }

    public void Setdatas(List<RandomPickUpTable.Data> pickUpdatas , List<bool> isNew , List<(bool, float)> isDuplication) 
    {
        for(int i = 0; i < 10; i++)
        {
            towerItemViewers[i].SetData(pickUpdatas[i] , isNew[i] , isDuplication[i]);
        }
    }
}
