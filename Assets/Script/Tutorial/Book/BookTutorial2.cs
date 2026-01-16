using UnityEngine;

public class BookTutorial2 : Tutorial
{
    private WindowManager windowManager;

    public override void TutorialEnter()
    {
        base.TutorialEnter();
        windowManager = GameObject.FindWithTag(TagIds.WindowManagerTag).GetComponent<WindowManager>();

        var window = windowManager.GetWindow(WindowIds.TitleBookWindow);
        if (window is TitleBookWindow titleBookWindow)
        {
            titleBookWindow.PlanetInfomationList[0].OnClickPlanet += OnClickPlanet;
            manager.SetTouchPlanelParent(titleBookWindow.PlanetInfomationList[0].transform);
        }
    }

    public override void TutorialExit()
    {
        base.TutorialExit();
        var window = windowManager.GetWindow(WindowIds.TitleBookWindow);
        if (window is TitleBookWindow titleBookWindow)
        {
            titleBookWindow.PlanetInfomationList[0].OnClickPlanet -= OnClickPlanet;
        }
    }

    public void OnClickPlanet(PlanetTable.Data data, PlanetInfomation info)
    {
        manager.SetNextTutorial();
    }
}