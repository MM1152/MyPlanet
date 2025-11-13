using System;
using TMPro;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlaceHold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeHoldCountText;

    private Image image;
    private TowerManager towerManager;
    private PopupManager popupManager;
    private bool placed = false;
    private int towerId;
    public int TowerId => towerId;

    public void Init(int index, int towerId , TowerManager towerManager , PopupManager poupManager)
    {
        this.towerManager = towerManager;
        this.popupManager = poupManager;

        image = GetComponent<Image>();

        placeHoldCountText.text = index.ToString();
        image.color = Color.gray;
        this.towerId = towerId;
    }

    public void SetPlace()
    {
        placed = true;
        image.color = Color.green;
    }

    public bool GetPlaced()
    {
        return placed;
    }

    private void Update()
    {
        if(Managers.TouchManager.TouchType == TouchTypes.LongTab && Managers.TouchManager.OnTargetUI(gameObject))
        {
            var popup = popupManager.Open<TowerInfomationPopup>(PopupIds.TowerInfomationPopup);
            popup.UpdateTexts(towerManager.GetIdToTower(TowerId));
        }
    }
}
