using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    [HideInInspector] public bool isInMenus;
    [HideInInspector] public bool isGamePlaying;

    [Header ("Battery")]
    public Slider batteryLevelSlider;
    public GameObject[] batteryUsageTiles;

    [Header("Screens")]
    public GameObject scannerScreen;
    public GameObject radarScreen;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject codeMenu;
    public GameObject emailMenu;
    public GameObject gameOverMenu;
    public GameObject victoryMenu;
    public GameObject backButton;

    [Header("Password")]
    public string password;

    private void Start()
    {
        if (CrossSceneInfo.victoryEmails)
        {
            isInMenus = true;
            isGamePlaying = false;
            Time.timeScale = 0f;
            emailMenu.SetActive(true);
            backButton.SetActive(true);
        } 
        else if (!CrossSceneInfo.restart)
        {
            isInMenus = true;
            isGamePlaying = false;
            Time.timeScale = 0f;
            mainMenu.SetActive(true);
        }
        else
        {
            isInMenus = false;
            isGamePlaying = true;
            Time.timeScale = 1f;
            backButton.SetActive(true);
        }
    }

    /// <summary>
    /// Updates the battery interface in fixed ranges.
    /// </summary>
    /// <param name="batteryLevel">Current value of the battery.(0 - 100)</param>
    public void UpdateBatteryLevelDiscret(float batteryLevel)
    {
        if(batteryLevel == 0)
        {
            batteryLevelSlider.value = 0;
        }
        else if (batteryLevel > 0 && batteryLevel <=20)
        {
            batteryLevelSlider.value = 20;
        }
        else if (batteryLevel > 20 && batteryLevel <= 40)
        {
            batteryLevelSlider.value = 40;
        }
        else if (batteryLevel > 40 && batteryLevel <= 60)
        {
            batteryLevelSlider.value = 60;
        }
        else if (batteryLevel > 60 && batteryLevel <= 80)
        {
            batteryLevelSlider.value = 80;
        }
        else if (batteryLevel > 80)
        {
            batteryLevelSlider.value = 100;
        }
    }
    /// <summary>
    /// Updates the battery interface to the exact level.
    /// </summary>
    /// <param name="batteryLevel">Current value of the battery.(0 - 100)</param>
    public void UpdateBatteryLevelContinuous(float batteryLevel)
    {
        batteryLevelSlider.value = batteryLevel;
    }

    /// <summary>
    /// Updates the battery usage on screen
    /// </summary>
    /// <param name="usage">Current usage of the battery. (0 - 5)</param>
    public void UpdateBatteryUsage(int usage)
    {
        print(usage);

        if (usage == 0)
        {
            if (batteryUsageTiles[0].activeInHierarchy)
            {
                batteryUsageTiles[0].SetActive(false);
            }
        }
        else if (usage == 1)
        {
            if (batteryUsageTiles[0].activeInHierarchy)
            {
                batteryUsageTiles[1].SetActive(false);
            }
            else
            {
                batteryUsageTiles[0].SetActive(true);
            }
        }
        else if (usage == 2)
        {
            if (batteryUsageTiles[1].activeInHierarchy)
            {
                batteryUsageTiles[2].SetActive(false);
            }
            else
            {
                batteryUsageTiles[1].SetActive(true);
            }
        }
        else if (usage == 3)
        {
            if (batteryUsageTiles[2].activeInHierarchy)
            {
                batteryUsageTiles[3].SetActive(false);
            }
            else
            {
                batteryUsageTiles[2].SetActive(true);
            }
        }
        else if (usage == 4)
        {
            if (batteryUsageTiles[3].activeInHierarchy)
            {
                batteryUsageTiles[4].SetActive(false);
            }
            else
            {
                batteryUsageTiles[3].SetActive(true);
            }
        }
        else if (usage == 5)
        {
            if (!batteryUsageTiles[4].activeInHierarchy)
            {
                batteryUsageTiles[4].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Switch the main screen elements to de Movement Screen.
    /// </summary>
    public void SwitchToMovementScreen()
    {
        scannerScreen.SetActive(false);
        radarScreen.SetActive(false);
        GameManager.GM.ToggleRadarMode(false);
        GameManager.GM.PC.isInMovementScreen = true;
    }

    /// <summary>
    /// Switch the main screen elements to de Scanner Screen.
    /// </summary>
    public void SwitchToScannerScreen()
    {
        scannerScreen.SetActive(true);
        radarScreen.SetActive(false);
        GameManager.GM.ToggleRadarMode(false);
        GameManager.GM.PC.isInMovementScreen = false;
    }

    /// <summary>
    /// Switch the main screen elements to de Sonar Screen.
    /// </summary>
    public void SwitchToRadarScreen()
    {
        radarScreen.SetActive(true);
        scannerScreen.SetActive(false);
        GameManager.GM.ToggleRadarMode(true);
        GameManager.GM.PC.isInMovementScreen = false;
    }

    public void ScannerPressed()
    {
        GameManager.GM.ScannerUsed();
    }

    public void RadarPressed()
    {
        GameManager.GM.SonarRadar();
    }

    public void StartMission ()
    {
        mainMenu.SetActive(false);

        backButton.SetActive(true);
        codeMenu.SetActive(true);
    }

    public void Restart()
    {
        CrossSceneInfo.restart = true;

        UIAudioManager.instance.PlayUIForward();

        SceneManager.LoadScene("MainLevel");
    }

    public void Emails()
    {
        mainMenu.SetActive(false);

        backButton.SetActive(true);
        emailMenu.SetActive(true);

        UIAudioManager.instance.PlayUIForward();
    }

    public void LogOut()
    {
        UIAudioManager.instance.PlayUIBack();
        Application.Quit();
    }

    public void CodeOk()
    {
        isInMenus = false;
        isGamePlaying = true;
        Time.timeScale = 1f;
        codeMenu.SetActive(false);
        backButton.SetActive(true);
    }

    public void Continue()
    {
        isInMenus = false;
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
        backButton.SetActive(true);
    }

    public void GameOver()
    {
        isInMenus = true;
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
        backButton.SetActive(false);
    }

    public void MainMenuButton()
    {
        CrossSceneInfo.restart = false;
        SceneManager.LoadScene("MainLevel");
    }

    public void Victory()
    {
        isInMenus = true;
        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
        backButton.SetActive(false);
    }

    public void VictoryEmail()
    {
        CrossSceneInfo.restart = false;
        CrossSceneInfo.victoryEmails = true;
        SceneManager.LoadScene("MainLevel");
    }

    public void Back()
    {
        codeMenu.SetActive(false);
        emailMenu.SetActive(false);
        backButton.SetActive(false);

        UIAudioManager.instance.PlayUIBack();

        isInMenus = true;
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
    }
}
