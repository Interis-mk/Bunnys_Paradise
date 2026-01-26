using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    private Button backButton;
    
    //TODO add options menu
    private void Start()
    {
        backButton = gameObject.GetComponent<Button>();
    }

    private void Update()
    {
        backButton.onClick.AddListener(BackToMainMenu);
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}