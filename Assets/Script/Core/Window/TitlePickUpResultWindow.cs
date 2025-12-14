using TMPro;
using UnityEngine;
using System.Collections.Generic;
public class TitlePickUpResultWindow : Window
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PlanetPickUpResult planetPickUpResult;
    [SerializeField] private RandomPickUp10Result planetPickUPResult10;
    [SerializeField] private TowerPickUpViewer towerPickUpResult;
    [SerializeField] private TowerPickUpResult towerPickUpResultViewer;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI panelText;

    private TowerTable.Data towerData;
    private PlanetTable.Data planetData;

    public override void Close()
    {
        base.Close();

        planetPickUpResult.gameObject.SetActive(false);
        planetPickUPResult10.gameObject.SetActive(false);
        towerPickUpResult.gameObject.SetActive(false);
        towerPickUpResultViewer.gameObject.SetActive(false);
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitlePickUpResultWindow;
    }

    public override void Open()
    {
        base.Open();
        panel.SetActive(true);
    }

    private void Update()
    {
        if(panel.activeSelf && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            panel.SetActive(false);
        }
    }

    public void SetData(RandomPickUpTable.Data randomData , bool isNew, (bool , float) isDuplication)
    {
        if (randomData.IsPlanetReward) panelText.text = "새로운 행성을 탐색중입니다.\n\nTouch!";
        if (randomData.IsTowerReward) panelText.text = "새로운 타워를 예 뭐.\n\nTouch!";

        if(randomData.IsPlanetReward)
        {
            planetPickUpResult.gameObject.SetActive(true);
            planetPickUpResult.SetData(randomData, isNew, isDuplication);
        }
    }

    public void SetData(List<RandomPickUpTable.Data> randomData ,List<bool> isNew, List<(bool ,float)> isDuplication)
    {
        if (randomData[0].IsPlanetReward) panelText.text = "새로운 행성을 탐색중입니다.\n\nTouch!";
        if (randomData[0].IsTowerReward) panelText.text = "새로운 타워를 예 뭐.\n\nTouch!";

        if (randomData[0].IsPlanetReward)
        {
            if(randomData.Count == 1)
            {
                SetData(randomData[0], isNew[0], isDuplication[0]);
                return;
            }
            planetPickUPResult10.gameObject.SetActive(true);
            planetPickUPResult10.SetData(randomData, isNew , isDuplication);
        }
        else if (randomData[0].IsTowerReward)
        {
            towerPickUpResult.gameObject.SetActive(true);
            towerPickUpResult.SetData(randomData, isNew , isDuplication);
        }
    }
}
