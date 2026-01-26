using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button optionsButton;
    public Button quitButton;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(OpenOptions);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }
    
    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Clicker-Base");
    }

    private void OpenOptions()
    {
        Debug.Log("Options button clicked!");
        SceneManager.LoadScene("Options");
    }

    private void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Quit button clicked!");
    }
}

