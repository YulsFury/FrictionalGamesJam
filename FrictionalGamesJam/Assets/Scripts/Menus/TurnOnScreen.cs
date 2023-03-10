using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnOnScreen : MonoBehaviour
{

    [Header("Delays")]
    public float delayBeforeTurnOn;
    public float delayAfterTurnOnSFX;
    public float delayBeforeInitialize;
    public float delayBeforeLogo;
    public float logoLerpTimeIn;
    public float logoLerpTimeOut;
    public float delayBeforeStartingGame;

    public float alphaLogo;
    public GameObject BlackScreen;
    public GameObject InitializingMenu;
    public Image logo;
    public Slider InitializingSlider;
    public float initializingTimer;
    private float initializingLevelUpdate;

    void Start()
    {
        var tempColor = logo.color;
        tempColor.a = 0f;
        logo.color = tempColor;
        StartCoroutine(TurnsOn());
        StartCoroutine(LogoAppears());
       
    }

    IEnumerator TurnsOn()
    {
        yield return new WaitForSeconds(delayBeforeTurnOn);
        AudioManager.instance.StartMonitorTurnOn();
        yield return new WaitForSeconds(delayAfterTurnOnSFX);
        GetComponent<Animator>().SetTrigger("TurnOnComputer");
    }

    IEnumerator LogoAppears()
    {
        yield return new WaitForSeconds(delayBeforeLogo);
        StartCoroutine(LogoLerp());
    }

    private void Initializing()
    {
        InitializingMenu.SetActive(true);
        InitializingSlider.maxValue = initializingTimer;
        StartCoroutine(InitializingTimer());
    }

    private IEnumerator WaitForInitializing()
    {
        yield return new WaitForSeconds(delayBeforeInitialize);
        Initializing();
    }

    public void DisableBlackScreen()
    {
        BlackScreen.SetActive(false);
    }

    private IEnumerator InitializingTimer()
    {
        initializingLevelUpdate = 0;
        while (initializingLevelUpdate < initializingTimer)
        {
            UpdateInitializingSlider(initializingLevelUpdate);
            initializingLevelUpdate += Time.deltaTime;
            yield return 0;
        }
        StartCoroutine(WaitForStartingGame());
    }

    public void UpdateInitializingSlider(float initializingLevel)
    {
        InitializingSlider.value = initializingLevel;
    }

    private IEnumerator WaitForStartingGame()
    {
        yield return new WaitForSeconds(delayBeforeStartingGame);
        AudioManager.instance.StopMonitorTurnOn();
        StartGame();
    }

    private void StartGame()
    {
        AudioManager.instance.PlayMusic();
        SceneManager.LoadScene("MainLevel");
    }

    IEnumerator LogoLerp()
    {

        yield return new WaitForSeconds(delayBeforeLogo);

        var alpha0 = logo.color;
        var alpha1 = logo.color;
        alpha1.a = alphaLogo;

        float timeLeft = logoLerpTimeIn;

        while (logo.color != alpha1)
        {
            if (timeLeft <= Time.deltaTime)
            {
                logo.color = alpha1;
            }
            else
            {
                logo.color = Color.Lerp(logo.color, alpha1, Time.deltaTime / timeLeft);

                timeLeft -= Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);

        timeLeft = logoLerpTimeOut;

        while (logo.color != alpha0)
        {
            if (timeLeft <= Time.deltaTime)
            {
                logo.color = alpha0;
            }
            else
            {
                logo.color = Color.Lerp(logo.color, alpha0, Time.deltaTime / timeLeft);

                timeLeft -= Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(WaitForInitializing());
    }
}
