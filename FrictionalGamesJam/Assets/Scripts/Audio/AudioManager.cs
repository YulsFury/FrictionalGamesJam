using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    public FMODUnity.EventReference uiConfirm;
    public FMODUnity.EventReference uiDeviceConect;
    public FMODUnity.EventReference uiDeviceDisconect;
    public FMODUnity.EventReference uiError;
    public FMODUnity.EventReference uiForward;
    public FMODUnity.EventReference uiGranted;
    public FMODUnity.EventReference uiNegative;
    public FMODUnity.EventReference uiOmnitoolConfigured;
    public FMODUnity.EventReference uiShutdown;
    public FMODUnity.EventReference uiStartUp;
    public FMODUnity.EventReference uiBack;
    public FMODUnity.EventReference openDoor;
    public FMODUnity.EventReference closeDoor;
    public FMODUnity.EventReference radar;
    public FMODUnity.EventReference playerDestination;

    public FMODUnity.EventReference enemyAlert;
    private FMOD.Studio.EventInstance enemyAlertInst;

    public FMODUnity.EventReference batteryOvertime;
    private FMOD.Studio.EventInstance batteryOvertimeInst;

    public FMODUnity.EventReference music;
    private FMOD.Studio.EventInstance musicInst;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayUIForward()
    {
        RuntimeManager.PlayOneShot(uiForward);
    }

    public void PlayUIConfirm()
    {
        RuntimeManager.PlayOneShot(uiConfirm);
    }
    public void PlayUIDeviceConect()
    {
        RuntimeManager.PlayOneShot(uiDeviceConect);
    }
    public void PlayUIDeviceDisconect()
    {
        RuntimeManager.PlayOneShot(uiDeviceDisconect);
    }
    public void PlayUIError()
    {
        RuntimeManager.PlayOneShot(uiError);
    }
    public void PlayPlayerDestination()
    {
        RuntimeManager.PlayOneShot(playerDestination);
    }
    public void PlayUIGranted()
    {
        RuntimeManager.PlayOneShot(uiGranted);
    }
    public void PlayUINegative()
    {
        RuntimeManager.PlayOneShot(uiNegative);
    }
    public void PlayUIOmnitoolConfigured()
    {
        RuntimeManager.PlayOneShot(uiOmnitoolConfigured);
    }
    public void PlayUIShutdown()
    {
        RuntimeManager.PlayOneShot(uiShutdown);
    }
    public void PlayUIStartUp()
    {
        RuntimeManager.PlayOneShot(uiStartUp);
    }
    public void PlayUIBack()
    {
        RuntimeManager.PlayOneShot(uiBack);
    }
    public void PlayOpenDoor()
    {
        RuntimeManager.PlayOneShot(openDoor);
    }
    public void PlayCloseDoor()
    {
        RuntimeManager.PlayOneShot(closeDoor);
    }

    public void PlayRadar()
    {
        RuntimeManager.PlayOneShot(radar);
    }

    public void PlayBatteryOvertime()
    {
        batteryOvertimeInst = FMODUnity.RuntimeManager.CreateInstance(batteryOvertime);
        batteryOvertimeInst.setParameterByNameWithLabel("Loop", "Play");
        batteryOvertimeInst.start();
    }

    public void StopBatteryOvertime()
    {
        batteryOvertimeInst.setParameterByNameWithLabel("Loop", "Silence");
    }

    public void PlayEnemyAlert()
    {
        enemyAlertInst = FMODUnity.RuntimeManager.CreateInstance(enemyAlert);
        enemyAlertInst.setParameterByName("Test", 0);
        enemyAlertInst.start();
    }
    public void PlayMusic()
    {
        musicInst = FMODUnity.RuntimeManager.CreateInstance(music);
        musicInst.setParameterByNameWithLabel("Music", "MainMenu");
        musicInst.start();
    }
    public void ChangeToMainMenuMusic()
    {
        musicInst.setParameterByNameWithLabel("Music", "MainMenu");
    }
    public void ChangeToMapMusic()
    {
        musicInst.setParameterByNameWithLabel("Music", "Map");
    }
    public void ChangeToGameOverMusic()
    {
        musicInst.setParameterByNameWithLabel("Music", "GameOver");
    }
    public void ChangeToVictoryMusic()
    {
        musicInst.setParameterByNameWithLabel("Music", "Victory");
    }
}
