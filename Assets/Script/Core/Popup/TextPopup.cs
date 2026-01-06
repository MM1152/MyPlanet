using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextPopup : Popup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private TextMeshProUGUI redButtonText;
    [SerializeField] private TextMeshProUGUI blueButtonText;

    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;

    public Button BlueButton => blueButton;
    public Button RedButton => redButton;

    public override bool Close()
    {
        redButton.onClick.RemoveAllListeners();
        blueButton.onClick.RemoveAllListeners();

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

    public void SetButtonAction(UnityAction blueButtonAction = null, UnityAction redButtonAction = null)
    {
        redButton.onClick.AddListener(redButtonAction);
        blueButton.onClick.AddListener(blueButtonAction);
    }

    public void SetButtonAudio(AudiosId blueButtonAudio = AudiosId.None, AudiosId redButtonAudio = AudiosId.None)
    {
        if (blueButtonAudio != AudiosId.None)
        {
            blueButton.onClick.AddListener(() => Managers.SoundManager.PlaySFX(blueButtonAudio));
        }

        if (redButtonAudio != AudiosId.None)
        {
            redButton.onClick.AddListener(() => Managers.SoundManager.PlaySFX(redButtonAudio));
        }
    }

}