using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    [HideInInspector] public bool isInMenus;
    [HideInInspector] public bool isGamePlaying;

    [Header("Data")]
    public GameObject dataTiles;

    [Header("Scanner")]
    public Slider ScannerCooldownSlider;
    public Button scannerButton;

    [Header ("Battery")]
    public Slider batteryLevelSlider;
    public GameObject[] batteryUsageTiles;

    [Header("Battery Level Colors")]
    public Color batteryLevelColor1;
    public Color batteryLevelColor2;
    public Color batteryLevelColor3;
    public Color batteryLevelColor4;
    public Color batteryLevelColor5;

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
        if (CrossSceneInfo.restartToEmails)
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
            GameManager.GM.DM.CloseInitialDoor();
            AudioManager.instance.ChangeToMapMusic();
            AudioManager.instance.PlayRobotOn();
        }

        ShowRadarWarning(false);

        int numOfChildren = dataTiles.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            dataTiles.transform.GetChild(i).GetComponent<Animator>().enabled = false;
        }
    }

    public void UpdateDataProgress(int keyItemsActivated, Color progressColor)
    {
        for (int i = 0; i < dataTiles.transform.childCount; i++)
        {
            if (i < keyItemsActivated)
            {
                Image DataTileImage = dataTiles.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>();
                DataTileImage.color = progressColor;
            }
        }
    }

    public void WarningDataNotCompleted()
    {
        int dataTilesCount = dataTiles.transform.childCount;
        int numberOfKeyItemsActivated = GameManager.GM.KIM.keyItemsActivated;
        for (int i = dataTilesCount - 1; i >= 0; i--)
        {
            Debug.Log(i);
            if(numberOfKeyItemsActivated <= i)
            {
                dataTiles.transform.GetChild(i).GetComponent<Animator>().enabled = true;
                dataTiles.transform.GetChild(i).GetComponent<Animator>().SetTrigger("NotCompleted");
            }
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
        UpdateBatterySliderColor();
    }
    /// <summary>
    /// Updates the battery interface to the exact level.
    /// </summary>
    /// <param name="batteryLevel">Current value of the battery.(0 - 100)</param>
    public void UpdateBatteryLevelContinuous(float batteryLevel)
    {
        batteryLevelSlider.value = batteryLevel;
        UpdateBatterySliderColor();
    }

    private void UpdateBatterySliderColor()
    {
        if (batteryLevelSlider.value == 0)
        {
            batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color = batteryLevelColor5;
        }
        else if (batteryLevelSlider.value > 0 && batteryLevelSlider.value <= 20)
        {
            batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color = batteryLevelColor4;
        }
        else if (batteryLevelSlider.value > 20 && batteryLevelSlider.value <= 40)
        {
            batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color = batteryLevelColor3;
        }
        else if (batteryLevelSlider.value > 40 && batteryLevelSlider.value <= 60)
        {
            batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color = batteryLevelColor2;
        }
        else if (batteryLevelSlider.value > 60 && batteryLevelSlider.value <= 80)
        {
            batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color = batteryLevelColor1;
        }
        else if (batteryLevelSlider.value > 80)
        {
            batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }

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
                ChangeBatterySliderColor(Color.white);
            }
        }
        else if (usage == 1)
        {
            if (batteryUsageTiles[0].activeInHierarchy)
            {
                batteryUsageTiles[1].SetActive(false);
                ChangeBatterySliderColor(batteryUsageTiles[0].GetComponent<Image>().color);
            }
            else
            {
                batteryUsageTiles[0].SetActive(true);
                ChangeBatterySliderColor(batteryUsageTiles[0].GetComponent<Image>().color);
            }
        }
        else if (usage == 2)
        {
            if (batteryUsageTiles[1].activeInHierarchy)
            {
                batteryUsageTiles[2].SetActive(false);
                ChangeBatterySliderColor(batteryUsageTiles[1].GetComponent<Image>().color);
            }
            else
            {
                batteryUsageTiles[1].SetActive(true);
                ChangeBatterySliderColor(batteryUsageTiles[1].GetComponent<Image>().color);
            }
        }
        else if (usage == 3)
        {
            if (batteryUsageTiles[2].activeInHierarchy)
            {
                ChangeBatterySliderColor(batteryUsageTiles[2].GetComponent<Image>().color);
                batteryUsageTiles[3].SetActive(false);
            }
            else
            {
                batteryUsageTiles[2].SetActive(true);
                ChangeBatterySliderColor(batteryUsageTiles[2].GetComponent<Image>().color);
            }
        }
        else if (usage == 4)
        {
            if (batteryUsageTiles[3].activeInHierarchy)
            {
                batteryUsageTiles[4].SetActive(false);
                ChangeBatterySliderColor(batteryUsageTiles[3].GetComponent<Image>().color);
            }
            else
            {
                batteryUsageTiles[3].SetActive(true);
                ChangeBatterySliderColor(batteryUsageTiles[3].GetComponent<Image>().color);
            }
        }
        else if (usage == 5)
        {
            if (!batteryUsageTiles[4].activeInHierarchy)
            {
                batteryUsageTiles[4].SetActive(true);
                ChangeBatterySliderColor(batteryUsageTiles[4].GetComponent<Image>().color);
            }
        }
    }

    public void UpdateScannerCooldownSlider(float scannerCooldownLevel)
    {
        ScannerCooldownSlider.value = scannerCooldownLevel;
    }

    public void ChangeBatterySliderColor(Color batteryUsageColor)
    {
        //var alpha = batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color.a;
        //batteryLevelSlider.transform.GetChild(0).GetComponent<Image>().color = new Color(batteryUsageColor.r, batteryUsageColor.g, batteryUsageColor.b, alpha);

        //foreach(Image batterySliderChild in batteryLevelSlider.transform.GetComponentsInChildren<Image>())
        //{
        //    var alpha = batterySliderChild.color.a;
        //    batterySliderChild.color = new Color(batteryUsageColor.r, batteryUsageColor.g, batteryUsageColor.b, alpha);
        //}
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
            GameManager.GM.PC.GetComponent<Scanner>().SwitchFinalRoomColor(true);
            GameManager.GM.PC.GetComponent<Scanner>().StopScanner();
            GameManager.GM.RM.UpdateRoomColorOfRooms();
            GameManager.GM.DM.UpdateRoomColorOfDoors();

            mapToggle.interactable = false;

            scannerToggle.interactable = true;
            scannerToggle.isOn = false;

            radarToggle.interactable = true;
            radarToggle.isOn = false;

            ActivateRadarWarning(true);

            GameManager.GM.EM.StartCounterChase();
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
            GameManager.GM.PC.GetComponent<Scanner>().SwitchFinalRoomColor(false);
            GameManager.GM.RM.ScannerRadarChangeRoomColorOfRooms();
            GameManager.GM.DM.ScannerRadarChangeColorOfDoors();

            mapToggle.interactable = true;
            mapToggle.isOn = false;

            scannerToggle.interactable = false;

            radarToggle.interactable = true;
            radarToggle.isOn = false;

            ActivateRadarWarning(true);

            GameManager.GM.EM.StopCounterChase();
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
            GameManager.GM.PC.GetComponent<Scanner>().SwitchFinalRoomColor(false);
            GameManager.GM.PC.GetComponent<Scanner>().StopScanner();
            GameManager.GM.RM.ScannerRadarChangeRoomColorOfRooms();
            GameManager.GM.DM.ScannerRadarChangeColorOfDoors();

            mapToggle.interactable = true;
            mapToggle.isOn = false;

            scannerToggle.interactable = true;
            scannerToggle.isOn = false;

            radarToggle.interactable = false;

            ActivateRadarWarning(false);

            GameManager.GM.EM.StopCounterChase();
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
        AudioManager.instance.PlayUIConfirm();
        AudioManager.instance.ChangeToMapMusic();
        if (AudioManager.instance.uiWhiteNoiseInst.isValid())
        {
            AudioManager.instance.StopUIWhiteNoise();
        }
        if (CrossSceneInfo.elementsUsingBattery > 0)
        {
            GameManager.GM.BC.HardStopOverTimeBattery();
        }
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
        isInMenus = false;
        isGamePlaying = true;
        Time.timeScale = 1f;
        codeMenu.SetActive(false);
        backButton.SetActive(true);
        AudioManager.instance.ChangeToMapMusic();
        AudioManager.instance.PlayRobotOn();
        GameManager.GM.DM.CloseInitialDoor();
    }

    public void Continue()
    {
        isInMenus = false;
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
        backButton.SetActive(true);
        AudioManager.instance.PlayUIConfirm();
        AudioManager.instance.ChangeToMapMusic();
        if (CrossSceneInfo.elementsUsingBattery > 0)
        {
            GameManager.GM.BC.ContinueBatteryUsage();
            AudioManager.instance.PlayBatteryOvertime();
        }
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

        if (CrossSceneInfo.elementsUsingBattery > 0)
        {
            GameManager.GM.BC.HardStopOverTimeBattery();
        }

        backButton.SetActive(false);
        AudioManager.instance.ChangeToGameOverMusic();
    }

    public void MainMenuButton()
    {
        CrossSceneInfo.restart = false;

        AudioManager.instance.PlayUIBack();
        AudioManager.instance.ChangeToMainMenuMusic();
        GameManager.GM.BC.HardStopOverTimeBattery();
        if (AudioManager.instance.uiWhiteNoiseInst.isValid())
        {
            AudioManager.instance.StopUIWhiteNoise();
            AudioManager.instance.StopBatteryOvertime();
        }

        SceneManager.LoadScene("MainLevel");
    }

    public void Victory()
    {
        isInMenus = true;
        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
        backButton.SetActive(false);
        if (CrossSceneInfo.elementsUsingBattery > 0)
        {
            GameManager.GM.BC.HardStopOverTimeBattery();
        }
        AudioManager.instance.ChangeToVictoryMusic();
    }

    public void VictoryEmail()
    {
        AudioManager.instance.PlayUIForward();
        CrossSceneInfo.restart = false;
        CrossSceneInfo.victoryEmails = true;
        CrossSceneInfo.restartToEmails = true;
        SceneManager.LoadScene("MainLevel");
    }

    public void Back()
    {
        if (!CrossSceneInfo.restartToEmails)
        {
            codeMenu.SetActive(false);
            emailMenu.SetActive(false);
            backButton.SetActive(false);

            AudioManager.instance.PlayUIBack();
            AudioManager.instance.ChangeToMainMenuMusic();
            AudioManager.instance.StopBatteryOvertime();
            GameManager.GM.BC.PauseBatteryUsage();

            isInMenus = true;
            Time.timeScale = 0f;
            mainMenu.SetActive(true);
        }
        else
        {
            CrossSceneInfo.restartToEmails = false;
            Time.timeScale = 1f;
            AudioManager.instance.StopMusic();
            AudioManager.instance.StopBatteryOvertime();
            SceneManager.LoadScene("Credits");
        }
        
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
