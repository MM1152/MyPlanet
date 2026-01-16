using UnityEngine;
using UnityEngine.UI;

public class TowerRandomPickUp2 : Tutorial
{
    public override void TutorialExit()
    {
        base.TutorialExit();
        manager.InitTutorial(TutorialStep.Book);
    }
}