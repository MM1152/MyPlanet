using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialTargetedButton : MonoBehaviour
{
    public int ButtonID;
    private Button button;
    private TutorialManager tutorialManager;
    private UnityAction callback;
    private bool init;

    private void Init()
    {
        init = true;
        button = GetComponent<Button>();
        tutorialManager = GameObject.FindWithTag(TagIds.TutorialManagerTag).GetComponent<TutorialManager>();
    }

    public void AddListner(UnityAction callback)
    {
        this.callback = callback;
        button.onClick.AddListener(this.callback);
    }

    public void UpdateButton()
    {
        if (!init) Init();
        button.onClick.AddListener(OnClickButton);
        button.interactable = true;
    }

    private void OnClickButton()
    {
        button.onClick.RemoveListener(OnClickButton);
        if(callback != null)
        {
            button.onClick.RemoveListener(callback);
            callback = null;
        }
        tutorialManager.SetNextTutorial();
    }
}
