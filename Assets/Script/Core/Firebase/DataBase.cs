using Cysharp.Threading.Tasks;
using Firebase.Database;

public class DataBase : Singleton<DataBase>
{
    private DatabaseReference root;

    static DataBase()
    {
        
    }

    private async UniTask Init()
    {
        await FirebaseInitalizer.Instance.WaitForInitalizedAsync();


    }
}