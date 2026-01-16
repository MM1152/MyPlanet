using Cysharp.Threading.Tasks;

public class BookTutorial5 : Tutorial
{
    private bool isFirstUpdate = false; 

    public override void TutorialEnter()
    {
        base.TutorialEnter();
        isFirstUpdate = false;
    }

    public override void TutorialUpdate()
    {
        if(!isFirstUpdate && manager.GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            manager.SetNextTutorial();
            SaveClearTutorialData();
        }
    }

    private void SaveClearTutorialData()
    {
        FirebaseManager.Instance.UserData.isClearRandomPickUpTutorial = true;
        var path = DataBasePaths.UserPath + FirebaseManager.Instance.UserId;
        FirebaseManager.Instance.UserData.SaveAsync(path , FirebaseManager.Instance.UserData).Forget();
    }
}