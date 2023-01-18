using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header ("Battery")]
    public Slider batterySlider;

    [Header("Screens")]
    public GameObject movementScreen;
    public GameObject sonarScreen;

    /// <summary>
    /// Updates de battery interface in fixed ranges.
    /// </summary>
    /// <param name="batteryLevel">Current value of the battery.(0 - 100)</param>
    public void UpdateBattery(float batteryLevel)
    {
        if(batteryLevel == 0)
        {
            batterySlider.value = 0;
        }
        else if (batteryLevel > 0 && batteryLevel <=20)
        {
            batterySlider.value = 20;
        }
        else if (batteryLevel > 20 && batteryLevel <= 40)
        {
            batterySlider.value = 40;
        }
        else if (batteryLevel > 40 && batteryLevel <= 60)
        {
            batterySlider.value = 60;
        }
        else if (batteryLevel > 60 && batteryLevel <= 80)
        {
            batterySlider.value = 80;
        }
        else if (batteryLevel > 80)
        {
            batterySlider.value = 100;
        }
    }

    /// <summary>
    /// Switch the main screen elements to de Movement Screen.
    /// </summary>
    public void SwitchToMovementScreen()
    {
        movementScreen.SetActive(true);
        sonarScreen.SetActive(false);        
    }

    /// <summary>
    /// Switch the main screen elements to de Sonar Screen.
    /// </summary>
    public void SwitchToSonarScreen()
    {
        sonarScreen.SetActive(true);
        movementScreen.SetActive(false);
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
