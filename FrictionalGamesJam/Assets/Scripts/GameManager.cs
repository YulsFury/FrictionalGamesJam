using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public InterfaceManager IM;
    public BatteryController BC;
    public NavMeshController NMC;
    public PlayerController PC;
    public EnemiesManager EM;
    public KeyItemsManager KIM;
    public RoomManager RM;
    public DoorManager DM;

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

        //DontDestroyOnLoad(GM);
    }

    public void ReduceBatteryLevelSingleTime(BatteryController.singleTimeSources source)
    {
        BC.DecreaseSingleTimeBattery(source);
    }

    public void ReduceBatteryOvertime(BatteryController.overtimeSources source)
    {
        BC.DecreaseOvertimeBattery(source);
    }
    public void StopReduceBatteryOvertime(BatteryController.overtimeSources source)
    {
        BC.StopUseOverTimeBattery(source);
    }
    public void ScannerUsed()
    {
        StartCoroutine(PC.GetComponent<Scanner>().ActiveScanner());
    }

    public void SonarRadar()
    {
        PC.GetComponent<Radar>().RadarInteraction();
    }

    public void ToggleRadarMode(bool activateRadar)
    {
        PC.GetComponent<Radar>().ToggleRadarMode(activateRadar);
    }

    public void GameOver(bool byEnemy)
    {
        IM.GameOver(byEnemy);
        BC.HardStopOverTimeBattery();
        AudioManager.instance.StartUIWhiteNoise();
    }

    public void Victory()
    {
        IM.Victory();
        BC.HardStopOverTimeBattery();
    }
}
