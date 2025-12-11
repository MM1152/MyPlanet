using TMPro;
using UnityEngine;

public class TitlePickUpResultWindow : Window
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PlanetPickUpResult planetPickUpResult;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI panelText;

    private TowerTable.Data towerData;
    private PlanetTable.Data planetData;

    public override void Close()
    {
        base.Close();

        planetPickUpResult.gameObject.SetActive(false);
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

    public void SetData(RandomPickUpTable.Data randomData , bool isNew)
    {
        if (randomData.IsPlanetReward) panelText.text = "새로운 행성을 탐색중입니다.\n\nTouch!";
        if (randomData.IsTowerReward) panelText.text = "새로운 타워를 예 뭐.\n\nTouch!";

        if(randomData.IsPlanetReward)
        {
            planetPickUpResult.gameObject.SetActive(true);
            planetPickUpResult.SetData(randomData, isNew);
        } 
    }
}
