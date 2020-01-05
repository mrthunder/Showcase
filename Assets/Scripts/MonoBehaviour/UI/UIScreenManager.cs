using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class UIScreenManager : Singleton<UIScreenManager>
{
    private Dictionary<System.Type, UIScreen> _menuScreens = new Dictionary<System.Type, UIScreen>();
    
    private Stack<UIScreen> _screenHistory = new Stack<UIScreen>();

    [SerializeField, SceneObjectsOnly]
    private UIScreen _firstScreen;

    [SerializeField, ReadOnly]
    private UIScreen _currentScreen;

    // Start is called before the first frame update
    void Start()
    {
        {
            var screens = FindObjectsOfType<UIScreen>();
            foreach (var screen in screens)
            {
                _menuScreens.Add(screen.GetType(), screen);
                screen.gameObject.SetActive(false);
            }
        }
        Show(_firstScreen);
    }

    public void Show<T>() where T:UIScreen
    {
        DeactivateCurrentScreen();
        _currentScreen = _menuScreens[typeof(T)];
        _currentScreen.gameObject.SetActive(true);
    }

    public void Show<T>(T _screen) where T : UIScreen
    {
        DeactivateCurrentScreen();
        _currentScreen = _screen;
        _currentScreen.gameObject.SetActive(true);
    }

    private void DeactivateCurrentScreen(bool savePrevious = true)
    {
        if (_currentScreen != null)
        {
            if(savePrevious)
            {
                _screenHistory.Push(_currentScreen);
            }
            _currentScreen.gameObject.SetActive(false);
            _currentScreen = null;
        }
    }

    public void BackOneScreen()
    {
        DeactivateCurrentScreen(false);
        _currentScreen = _screenHistory?.Pop();
        _currentScreen?.gameObject.SetActive(true);
    }


    
}
