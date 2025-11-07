using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Button anymousLoginButton;
    public TextMeshProUGUI uidText;

    private void Awake()
    {
        anymousLoginButton.onClick.AddListener(async () =>
        {
            await FirebaseManager.Instance.WaitForInitalizedAsync();
            var (id, success) = await FirebaseManager.Instance.Auth.SignInAnonymouslyAsync();
            if (success)
            {
                uidText.text = $"User ID: {id}";
            }
        });
    }

    private async void Start()
    {
        await FirebaseManager.Instance.WaitForInitalizedAsync();
        var data = await FirebaseManager.Instance.Database.GetDatas<User>("users");
        if(data.success)
        {
            foreach (var dat in data.data)
            {
                Debug.Log(dat.id + " " + dat.value);
            }
        }

        if (FirebaseManager.Instance.Auth.UserId != string.Empty)
        {
            uidText.text = $"User ID: {FirebaseManager.Instance.Auth.UserId}";
        }
        User userData = new User("Ãµ¹Î¼º" , 11);
        var setting = await FirebaseManager.Instance.Database.OverwriteJsonData($"users/{FirebaseManager.Instance.Auth.UserId}" , userData);
        Debug.Log(setting);

    }
}
