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

    [Header("Screens Toggles")]
    public Toggle mapToggle;
    public Toggle scannerToggle;
    public Toggle radarToggle;

    [Header("Radar Warning")]
    public GameObject warningRadarImage;
    private bool isWarningRadarShown;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject codeMenu;
    public GameObject emailMenu;
    public GameObject gameOverMenu;
    public GameObject victoryMenu;
    public GameObject backButton;

    [Header("Password")]
    public string password;

    [Header("Game Over")]
    public GameObject gameOverByEnemyText;
    public GameObject gameOverByEnergyText;

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
            AudioManager.instance.ChangeToMainMenuMusic();
        }
        else
        {
            isInMenus = false;
            isGamePlaying = true;
            Time.timeScale = 1f;
            backButton.SetActive(true);
            AudioManager.instance.ChangeToMapMusic();
        }

        ShowRadarWarning(false);
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
        if (mapToggle.isOn)
        {
            scannerScreen.SetActive(false);
            radarScreen.SetActive(false);
            GameManager.GM.ToggleRadarMode(false);
            GameManager.GM.PC.isInMovementScreen = true;
            AudioManager.instance.PlayUIForward();
            GameManager.GM.RM.UpdateRoomColorOfRooms();
            GameManager.GM.DM.UpdateRoomColorOfDoors();

            mapToggle.interactable = false;

            scannerToggle.interactable = true;
            scannerToggle.isOn = false;

            radarToggle.interactable = true;
            radarToggle.isOn = false;

            ActivateRadarWarning(true);
        }
    }

    /// <summary>
    /// Switch the main screen elements to de Scanner Screen.
    /// </summary>
    public void SwitchToScannerScreen()
    {
        if (scannerToggle.isOn)
        {
            scannerScreen.SetActive(true);
            radarScreen.SetActive(false);
            GameManager.GM.ToggleRadarMode(false);
            GameManager.GM.PC.isInMovementScreen = false;
            AudioManager.instance.PlayUIForward();
            GameManager.GM.RM.ScannerRadarChangeRoomColorOfRooms();
            GameManager.GM.DM.ScannerRadarChangeColorOfDoors();

            mapToggle.interactable = true;
            mapToggle.isOn = false;

            scannerToggle.interactable = false;

            radarToggle.interactable = true;
            radarToggle.isOn = false;

            ActivateRadarWarning(true);
        }
    }

    /// <summary>
    /// Switch the main screen elements to de Radar Screen.
    /// </summary>
    public void SwitchToRadarScreen()
    {
        if (radarToggle.isOn)
        {
            radarScreen.SetActive(true);
            scannerScreen.SetActive(false);
            GameManager.GM.ToggleRadarMode(true);
            GameManager.GM.PC.isInMovementScreen = false;
            AudioManager.instance.PlayUIForward();
            GameManager.GM.RM.ScannerRadarChangeRoomColorOfRooms();
            GameManager.GM.DM.ScannerRadarChangeColorOfDoors();

            mapToggle.interactable = true;
            mapToggle.isOn = false;

            scannerToggle.interactable = true;
            scannerToggle.isOn = false;

            radarToggle.interactable = false;

            ActivateRadarWarning(false);
        }
    }

    public void ScannerPressed()
    {
        GameManager.GM.ScannerUsed();
        AudioManager.instance.PlayUIForward();
    }

    public void RadarPressed()
    {
        GameManager.GM.SonarRadar();
        AudioManager.instance.PlayUIForward();
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
        gameOverMenu.SetActive(false);
        AudioManager.instance.PlayUIForward();
        AudioManager.instance.ChangeToMapMusic();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainLevel");
    }

    public void Emails()
    {
        mainMenu.SetActive(false);

        backButton.SetActive(true);
        emailMenu.SetActive(true);

        AudioManager.instance.PlayUIForward();
    }

    public void LogOut()
    {
        AudioManager.instance.PlayUIBack();
        CrossSceneInfo.playingMusic = false;
        Application.Quit();
    }

    public void CodeOk()
    {
        print("CodeOK");
        isInMenus = false;
        isGamePlaying = true;
        Time.timeScale = 1f;
        codeMenu.SetActive(false);
        backButton.SetActive(true);
        AudioManager.instance.ChangeToMapMusic();
    }

    public void Continue()
    {
        isInMenus = false;
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
        backButton.SetActive(true);
        AudioManager.instance.PlayUIForward();
    }

    public void GameOver(bool byEnemy)
    {
        isInMenus = true;
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);

        if (byEnemy)
        {
            gameOverByEnemyText.SetActive(true);
            gameOverByEnergyText.SetActive(false);
        }
        else
        {
            gameOverByEnemyText.SetActive(false);
            gameOverByEnergyText.SetActive(true);
        }

        backButton.SetActive(false);
        AudioManager.instance.ChangeToGameOverMusic();
    }

    public void MainMenuButton()
    {
        CrossSceneInfo.restart = false;
        AudioManager.instance.PlayUIBack();
        AudioManager.instance.ChangeToMainMenuMusic();
        SceneManager.LoadScene("MainLevel");
    }

    public void Victory()
    {
        isInMenus = true;
        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
        backButton.SetActive(false);
        AudioManager.instance.ChangeToVictoryMusic();
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

        AudioManager.instance.PlayUIBack();

        isInMenus = true;
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
    }

    public void ShowRadarWarning(bool activate)
    {
        isWarningRadarShown = activate;
    }

    public void ActivateRadarWarning(bool activate)
    {
        if (isWarningRadarShown)
        {
            warningRadarImage.SetActive(activate);
        }
    }
}
