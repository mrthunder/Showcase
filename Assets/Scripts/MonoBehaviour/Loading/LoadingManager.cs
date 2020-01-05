using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : Singleton<LoadingManager>
{
    private const string TitleText = "Loading...";
    [Title("UI"),SceneObjectsOnly]
    public TMP_Text PercentageText;
    [SceneObjectsOnly]
    public TMP_Text LoadingTitle;
    [SceneObjectsOnly]
    public Image LoadingBar;

    private int length = 0;

    private AsyncOperation _asyncOperation;
    private static string _nextScene;

    private WaitForSeconds effectDelay = new WaitForSeconds(0.2f);

    // Start is called before the first frame update
    void Start()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(_nextScene);
        StartCoroutine(LoadingTitleEffect());
        _asyncOperation.allowSceneActivation = true;
        
    }

   

    private IEnumerator LoadingTitleEffect()
    {
        while(!_asyncOperation.isDone)
        {
            length = (length + 1) % (TitleText.Length+1);
            yield return effectDelay;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(_asyncOperation != null)
        {
            PercentageText.text = $"{_asyncOperation.progress*100}%";
            LoadingBar.fillAmount = _asyncOperation.progress;
            LoadingTitle.text = TitleText.Substring(0, length);
        }
        else
        {
            Debug.Log("Async Operation is null");
        }
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("Loading");
        _nextScene = sceneName;
    }
}
