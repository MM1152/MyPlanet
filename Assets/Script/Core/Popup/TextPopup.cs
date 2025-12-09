using TMPro;
using UnityEngine;

public class TextPopup : Popup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private TextMeshProUGUI redButtonText;
    [SerializeField] private TextMeshProUGUI blueButtonText;

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.TextPopup;
    }

    public override void Open()
    {
        base.Open();
    }

    public void SetTexts(string title = "" , string body = "" , string redButtonText = "" , string blueButtonText = "")
    {
        this.titleText.text = title;
        this.bodyText.text = body;
        this.redButtonText.text = redButtonText;
        this.blueButtonText.text = blueButtonText;
    }
}