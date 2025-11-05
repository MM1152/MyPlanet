using Firebase.Auth;

public class Auth
{
    private FirebaseAuth auth;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
    }
}

