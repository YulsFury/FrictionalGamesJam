using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public Slider batterySprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates de battery interface in fixed ranges.
    /// </summary>
    /// <param name="batteryLevel">Current value of the battery.(0 - 100)</param>
    public void UpdateBattery(float batteryLevel)
    {
        if(batteryLevel == 0)
        {
            batterySprite.value = 0;
        }
        else if (batteryLevel > 0)
        {
            batterySprite.value = 20;
        }
        else if (batteryLevel > 20)
        {
            batterySprite.value = 40;
        }
        else if (batteryLevel > 40)
        {
            batterySprite.value = 60;
        }
        else if (batteryLevel > 60)
        {
            batterySprite.value = 80;
        }
        else if (batteryLevel > 80)
        {
            batterySprite.value = 100;
        }
    }
}
