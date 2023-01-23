using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMPro.TMP_Text startButtonText;
    public Button resetButton;

    private void OnEnable()
    {
        if (GameManager.GM.IM.isGamePlaying)
        {
            startButtonText.text = "CONTINUE";
            resetButton.interactable = true;
        }
        else
        {
            startButtonText.text = "START MISSION";
            resetButton.interactable = false;
        }
    }

    public void StartOrContinue()
    {
        InterfaceManager IM = GameManager.GM.IM;

        if (IM.isGamePlaying)
        {
            IM.Continue();
            AudioManager.instance.PlayUIForward();
        }
        else
        {
            IM.StartMission();
            AudioManager.instance.PlayUIForward();
        }
    }
}
