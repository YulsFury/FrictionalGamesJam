using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public InterfaceManager IM;
    public BatteryController BC;
    public NavMeshController NMC;
    public PlayerController PC;

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

    public void ReduceBatteryLevel()
    {
        BC.DecreaseSingleTimeBattery();
    }

    public void ReduceBatteryOvertime()
    {
        BC.DecreaseOvertimeBattery();
    }

    public void StopReducingBatteryOvertime()
    {
        BC.StopUseOverTimeBattery();
    }

    public void GameOver()
    {
        Debug.LogWarning("GameOver");
    }
}
