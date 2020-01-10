using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : UIScreen
{
    [Title("Register"), SerializeField, ChildGameObjectsOnly]
    private TMP_InputField _username;
    [SerializeField, ChildGameObjectsOnly]
    private TMP_InputField _password;
    [SerializeField, ChildGameObjectsOnly]
    private TMP_Text _resultText;
    [SerializeField, ChildGameObjectsOnly]
    private Image _loadingCircle;

    private bool _isRegistering = false;
    private bool IsRegistering
    {
        get
        {
            return _isRegistering;
        }
        set
        {
            _isRegistering = value;
            _loadingCircle.gameObject.SetActive(value);
        }
    }


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
        if (IsRegistering) return;
        _resultText.text = "";
        IsRegistering = true;
        (string username, string password) user = (_username.text, _password.text);
        MongoManager.Instance.RegisterUser(user, result =>
        {
            IsRegistering = false;
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
