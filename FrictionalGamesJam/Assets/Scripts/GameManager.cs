using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GC;
    public InterfaceManager IC;
    public BatteryController BC;

    private void Awake()
    {
        if(GC != null)
        {
            GameManager.Destroy(GC);
        }
        else
        {
            GC = this;
        }

        DontDestroyOnLoad(GC);
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
