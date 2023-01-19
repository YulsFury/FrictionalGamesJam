using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header ("Battery")]
    public Slider batteryLevelSlider;
    public GameObject[] batteryUsageTiles;

    [Header("Screens")]
    public GameObject movementScreen;
    public GameObject sonarScreen;

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
        movementScreen.SetActive(true);
        sonarScreen.SetActive(false);
        GameManager.GM.ToggleSonarMode(false);
    }

    /// <summary>
    /// Switch the main screen elements to de Sonar Screen.
    /// </summary>
    public void SwitchToSonarScreen()
    {
        sonarScreen.SetActive(true);
        movementScreen.SetActive(false);
        GameManager.GM.ToggleSonarMode(true);
    }

    public void ScannerPressed()
    {
        GameManager.GM.ScannerUsed();
    }

    public void SonarPressed()
    {
        GameManager.GM.SonarUsed();
    }
}
