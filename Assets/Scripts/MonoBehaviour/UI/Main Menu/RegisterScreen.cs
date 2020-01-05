using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class RegisterScreen : UIScreen
{
    [Title("Register"), SerializeField, ChildGameObjectsOnly]
    private TMP_InputField _username;
    [SerializeField, ChildGameObjectsOnly]
    private TMP_InputField _password;
    [SerializeField, ChildGameObjectsOnly]
    private TMP_Text _resultText;

    private void OnDisable()
    {
        _username.text = "";
        _password.text = "";
        _resultText.text = "";
    }

    public void GoBack()
    {
        UIScreenManager.Instance.BackOneScreen();
    }

    public void Register()
    {
        _resultText.text = "";
        (string username, string password) user = (_username.text, _password.text);
        MongoManager.Instance.RegisterUser(user, result =>
        {
            if (result)
            {
                UIScreenManager.Instance.BackOneScreen();
            }
            else
            {
                _resultText.text = "<color=red>Username already exist.</color>";
                
            }

        });
    }
}
