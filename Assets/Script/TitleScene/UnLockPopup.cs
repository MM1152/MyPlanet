using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnLockPopup : Popup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button unlockButton;

    public override bool Close()
    {
        unlockButton.onClick.RemoveAllListeners();
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        cancelButton.onClick.AddListener(() => manager.ForceClose());
        popupId = (int)PopupIds.UnLockPopup;
    }

    public override void Open()
    {
        base.Open();
    }

    public void Setting(int idx , Action<int> unlockAction)
    {
        titleText.text = $"현재 슬릇 : {idx}\n슬릇을 해금하시겠습니까?";
        unlockButton.onClick.AddListener(() =>
        {
            unlockAction?.Invoke(idx);
            manager.ForceClose();
        });
    }
}
