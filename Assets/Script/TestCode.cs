using UnityEngine;

public class User : JsonSerialized
{
    public string id;
    public int value;

    public User(string id , int value)
    {
        this.id = id;
        this.value = value;
    }
}

public class TestCode : MonoBehaviour 
{
    
    private async void Start()
    {
        await FirebaseManager.Instance.WaitForInitalizedAsync();
        User user = new User("asd" , 1);
        bool isSuccess = await FirebaseManager.Instance.Database.PushWirteJsonData<User>("users" , user);
        Debug.Log(isSuccess);
        var data = await FirebaseManager.Instance.Database.GetData<User>("users");
        Debug.Log(data.data.id +  " " + data.data.value);
    }
}
