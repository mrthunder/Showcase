using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is not a persistent singleton, that being said, it will get destroyed with the scene
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance = null;
    public static T Instance => _instance ?? (_instance = FindObjectOfType<T>());


    private void OnDestroy()
    {
        if(_instance != null)
        {
            DestroyImmediate(_instance);
        }
    }
}
