using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class LoginScreen : UIScreen
{
    [Title("Login"), SerializeField,ChildGameObjectsOnly(IncludeSelf =true)]
    private TMP_InputField _username;
    [SerializeField, ChildGameObjectsOnly(IncludeSelf = true)]
    private TMP_InputField _password;
    [SerializeField, ChildGameObjectsOnly]
    private TMP_Text _resultText;

    private void OnDisable()
    {
        _username.text = "";
        _password.text = "";
        _resultText.text = "";
    }

    public void GotoRegister()
    {
        UIScreenManager.Instance.Show<RegisterScreen>();
    }

    public void Login()
    {
        _resultText.text = "";
        (string username, string password) user = (_username.text, _password.text);
        MongoManager.Instance.ConnectUser(user, result =>
        {
            if(result)
            {
                LoadingManager.LoadScene("Forest");
            }
            else
            {
                _resultText.text = "<color=red>Username or Password might be wrong.</color>";
            }
            
        });
    }
}
