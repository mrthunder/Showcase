using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : UIScreen
{
    
    public void GoToLoginScreen()
    {
        UIScreenManager.Instance.Show<LoginScreen>();
    }
}
