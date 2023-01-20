using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string LevelName;
    public Button startButton;
    public TMPro.TMP_InputField passwordInput;

    public string password;

    public void StartGame()
    {
        SceneManager.LoadScene(LevelName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CheckPassword()
    {
        if(password == passwordInput.text)
        {
            startButton.interactable = true;
        }
    }
}
