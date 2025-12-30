using UnityEngine;

public class InGamePlaceTowerWindow : TitleTowerPlaceEditWindow
{
    public override void Init(WindowManager manager)
    {
        this.manager = manager;
        windowId = (int)WindowIds.InGamePlaceTowerWindow;

        Canvas.ForceUpdateCanvases();
        circleSize = new Vector3(circle.rectTransform.rect.width, circle.rectTransform.rect.height);

        inGameViewer = true;

        SetPresetData(FirebaseManager.Instance.PresetData.GetGameData().data , 0);
    }
    public override void Close()
    {
        return;
    }
}