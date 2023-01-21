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

    public void ScannerUsed()
    {
        StartCoroutine(PC.GetComponent<Scanner>().ActiveScanner());
    }

    public void SonarUsed()
    {
        PC.GetComponent<Sonar>().SonarInteraction();
    }

    public void ToggleSonarMode(bool activateSonar)
    {
        PC.GetComponent<Sonar>().ToggleSonarMode(activateSonar);
        PC.isUsingSonar = activateSonar;
    }

    public void GameOver()
    {
        IM.GameOver();
    }

    public void Victory()
    {
        IM.Victory();
    }
}
