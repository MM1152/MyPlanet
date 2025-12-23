using Cysharp.Threading.Tasks;

public class BookTutorial5 : Tutorial
{
    private string msg = "전투에 진입하지 않아도\n이곳에서 배치 전략을 짤 수 있습니다.";
    private bool isFirstUpdate = false;
    public override void TutorialEnter()
    {
        var clip = GetClip(2, 22);
        SetTextWithAnimation(msg , clip).Forget();
        isFirstUpdate = false;
    }

    public override void TutorialExit()
    {

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