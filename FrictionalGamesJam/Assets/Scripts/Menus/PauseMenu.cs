using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isGamePaused;

    public GameObject MenuButton;
    public GameObject QuitButton;

    public string mainMenuName;

    void Start()
    {
        isGamePaused = false;
        MenuButton.SetActive(false);
        QuitButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        MenuButton.SetActive(false);
        QuitButton.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    private void Pause()
    {
        MenuButton.SetActive(true);
        QuitButton.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
