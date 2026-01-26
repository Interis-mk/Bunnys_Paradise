using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public static SceneLoad instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            SceneManager.LoadScene(name);
        }
        else
        {
            Debug.LogWarning("SceneLoaderButton: sceneToLoad is empty.");
        }
    }
}