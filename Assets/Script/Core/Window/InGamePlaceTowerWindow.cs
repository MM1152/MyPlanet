using UnityEngine;
using System.Collections.Generic;
public class InGamePlaceTowerWindow : TitleTowerPlaceEditWindow
{
    [SerializeField] private List<TerraformingUI> terraformingStateObjects;

    public override void Init(WindowManager manager)
    {
        this.manager = manager;
        windowId = (int)WindowIds.InGamePlaceTowerWindow;

        Canvas.ForceUpdateCanvases();
        circleSize = new Vector3(circle.rectTransform.rect.width, circle.rectTransform.rect.height);

        inGameViewer = true;

        SetPresetData(FirebaseManager.Instance.PresetData.GetGameData().data, 0);
    }

    public void SetTerraformingState(int point, TerraformingTable.Data data)
    {
        if (point >= terraformingStateObjects.Count)
            return;

        terraformingStateObjects[point].SetUI(data);
    }
    
    public override void Close()
    {
        return;
    }
}