using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PresetWindowTutorial : Tutorial
{
    private List<Button> interactionButton = new List<Button>();
    private Button backButton;
    public override void TutorialEnter()
    {
        var objects = GameObject.FindGameObjectsWithTag(TagIds.TutorialTaget);
        backButton = GameObject.FindGameObjectWithTag(TagIds.BackButton).GetComponent<Button>();

        backButton.onClick.AddListener(OnClickBackButton);

        for (int i = 0; i < objects.Length; i++)
        {
            var button = objects[i].GetComponent<Button>();
            if(button != null)
            {
                interactionButton.Add(button);
                button.onClick.AddListener(OnClickInteractionButton);
            }
        }

        Canvas.ForceUpdateCanvases();

        manager.SetTouchPlanelParent(interactionButton[0].transform);
        manager.SetTextAreaPosition(3);

        var clip = GetCombineClip(4, 0 , 4 , 1);
        SetTextWithAnimation(DataTableManager.StringTable.Get(6238), clip , backGroundRayCastAble : false).Forget();

        Debug.Log("Start Tutorial Preset 1");
    }

    public override void TutorialUpdate()
    {

    }

    public override void TutorialExit()
    {
        manager.SetTutorialBackGround(true);

        foreach (var button in interactionButton)
        {
            button.onClick.RemoveListener(OnClickInteractionButton);
        }

        backButton.onClick.RemoveListener(OnClickBackButton);

        Debug.Log("Exit Tutorial Preset 1");
    }

    private void OnClickInteractionButton()
    {
        manager.SetNextTutorial();
    }

    private void OnClickBackButton()
    {
        manager.SetPrevTutorial();
    }
}