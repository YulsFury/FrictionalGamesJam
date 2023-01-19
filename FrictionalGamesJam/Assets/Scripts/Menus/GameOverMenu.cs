using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public string levelName;
    public string mainMenuName;

    public void Restart()
    {
        SceneManager.LoadScene(levelName);
    }

    public void Menu()
    {
        SceneManager.LoadScene(mainMenuName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
