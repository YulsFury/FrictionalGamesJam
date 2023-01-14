using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public InterfaceManager IM;
    public BatteryController BC;

    private void Awake()
    {
        if(GM != null)
        {
            GameManager.Destroy(GM);
        }
        else
        {
            GM = this;
        }

        DontDestroyOnLoad(GM);
    }

    public void ReduceBatteryLevel(float decreasedAmmount)
    {
        BC.DecreaseSingleTimeBattery(decreasedAmmount);
    }

    public void ReduceBatteryOvertime(float decreasedAmmount)
    {
        BC.DecreaseOvertimeBattery(decreasedAmmount);
    }
}
