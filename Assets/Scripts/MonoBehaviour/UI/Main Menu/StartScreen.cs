using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : UIScreen
{
    
    public void GoToLoginScreen()
    {
        //I am bypassing the login screen to go directly to the game
        LoadingManager.LoadScene("Forest");

        //UIScreenManager.Instance.Show<LoginScreen>();
    }
}
