using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneHelper
{
    public static void LoadScene(string scene, bool additive = false, bool setActive = false)
    {
        if (scene == null)
        {
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            scene, additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : 0);

        if (setActive)
        {
            // to mark it active we have to wait a frame for it to load.
            TopDown.Interaction.SceneLoading.CallAfterDelay.Create(0, () =>
            {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(
                    UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene));
            });
        }
    }

    public static void UnloadScene(string s)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s);
    }
}