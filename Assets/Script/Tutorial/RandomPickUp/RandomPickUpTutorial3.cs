using UnityEngine;
using UnityEngine.UI;

public class RandomPickUpTutorial3 : Tutorial
{
    public override void TutorialExit()
    {
        manager.InitTutorial(TutorialStep.PickUp2);
    }
}   