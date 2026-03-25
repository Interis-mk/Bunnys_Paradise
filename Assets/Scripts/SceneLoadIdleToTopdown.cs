using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadIdleToTopdown : MonoBehaviour
{
    // Set this in the Inspector (must match the scene name in Build Settings)
    [SerializeField] private string sceneToLoad;

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("SceneLoaderButton: sceneToLoad is empty.");
        }
    }
}