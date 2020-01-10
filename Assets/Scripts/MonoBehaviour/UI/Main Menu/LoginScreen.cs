using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class LoginScreen : UIScreen
{
    [Title("Login"), SerializeField,ChildGameObjectsOnly(IncludeSelf =true)]
    private TMP_InputField _username;
    [SerializeField, ChildGameObjectsOnly(IncludeSelf = true)]
    private TMP_InputField _password;
    [SerializeField, ChildGameObjectsOnly]
    private TMP_Text _resultText;
    [SerializeField,ChildGameObjectsOnly]
    private Image _loadingCircle;

    private bool _isLogin = false;
    private bool IsLogin
    {
        get
        {
            return _isLogin ;
        } 
        set 
        {
            _isLogin = value;
            _loadingCircle.gameObject.SetActive(value);
        } 
    }

    [SerializeField,BoxGroup("Debug")]
    private bool _offline = false;
    


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
        _resultText.text = "Login method";
        if (IsLogin) return;
        //_resultText.text = "";
        if (_offline == false)
        {
            IsLogin = true;
            (string username, string password) user = (_username.text, _password.text);
            MongoManager.Instance.ConnectUser(user, result =>
            {
                IsLogin = false;
                if (result)
                {   
                    LoadingManager.LoadScene("Forest");
                }
                else
                {
                    _resultText.text = "<color=red>Username or Password might be wrong.</color>";
                }

            });
        }
        else
        {
            LoadingManager.LoadScene("Forest");
        }
    }
}
