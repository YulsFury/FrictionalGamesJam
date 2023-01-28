using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CreditsScreen : MonoBehaviour
{
    [Header ("Video")]
    public GameObject videoTurnOff;

    [Header("Background")]
    public float delayBeforeDarkening;
    public GameObject darkenBackground;
    public GameObject Monitor;
    public GameObject TurnOffMonitor;
    public GameObject TurnOnMonitor;
    public int numberOfLightFlicks;
    public float timeBetweenLightFlicks;

    [Header("TurnOnComputer")]
    public float delayBeforeTurnOnComputer;

    [Header ("Logo")]
    public float delayBeforeLogo;
    public float alphaLogo;
    public Image logo;
    public float lerpTime;

    [Header ("Command Shell")]
    public float delayBeforeShell;
    public TMPro.TMP_Text fullText;
    public TMPro.TMP_Text textShown;
    public float delayChunck;
    public float delayLetter;

    void Start()
    {
        StartCoroutine(TurnOffLights());
        Time.timeScale = 1f;
        textShown.text = "";
        videoTurnOff.GetComponent<VideoPlayer>().loopPointReached += EndOfVideo;
        var tempColor = logo.color;
        tempColor.a = 0f;
        logo.color = tempColor;
    }

    void EndOfVideo(VideoPlayer vp)
    {
        videoTurnOff.SetActive(false);
        
    }

    IEnumerator TurnOffLights()
    {
        AudioManager.instance.PlayPowerOff();
        AudioManager.instance.PlayMonitorOff();
        yield return new WaitForSeconds(delayBeforeDarkening);

        for (int i = 0; i < numberOfLightFlicks; i++)
        {
            yield return new WaitForSeconds(timeBetweenLightFlicks);

            darkenBackground.SetActive(true);
            TurnOffMonitor.SetActive(true);
            Monitor.SetActive(false);

            yield return new WaitForSeconds(timeBetweenLightFlicks);

            Monitor.SetActive(true);
            darkenBackground.SetActive(false);
            TurnOffMonitor.SetActive(false);
            
        }

        darkenBackground.SetActive(true);
        TurnOffMonitor.SetActive(true);
        Monitor.SetActive(false);

        StartCoroutine(TurnOnComputer());
        StartCoroutine(LogoLerp());
    }

    IEnumerator TurnOnComputer()
    {
        yield return new WaitForSeconds(delayBeforeTurnOnComputer);
        AudioManager.instance.PlayRobotOn();
        Monitor.SetActive(false);
        TurnOnMonitor.SetActive(true);
    }
    IEnumerator LogoLerp()
    {

        yield return new WaitForSeconds(delayBeforeLogo);

        TurnOffMonitor.SetActive(false);

        var alpha0 = logo.color;
        var alpha1 = logo.color;
        alpha1.a = alphaLogo;

        float timeLeft = lerpTime;

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

        timeLeft = lerpTime;

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

        //var ColorAuxiliar = logo.color;
        //ColorAuxiliar.a = 0f;
        //logo.color = ColorAuxiliar;

        StartCoroutine(PrintText());
    }


    IEnumerator PrintText()
    {

        yield return new WaitForSeconds(delayBeforeShell);

        string realPath = Application.dataPath;
        string[] allPathPieces = realPath.Split("/");
        string path = "";

        for(int i = 0; i < 3; i++)
        {
            path += i != 2 ? allPathPieces[i] + "\\" : allPathPieces[i];
        }

        string[] allPieces = fullText.text.Split('%');

        foreach(string piece in allPieces)
        {
            yield return new WaitForSeconds(delayChunck);

            if (piece.Length > 0)
            {
                if (piece.StartsWith("$"))
                {
                    AudioManager.instance.PlayConsoleLetter();

                    string s = piece.Remove(0, 1);

                    if (s == "#")
                    {
                        textShown.text += path + ">";
                    }
                    else
                    {
                        textShown.text += s;
                    }
                }
                else
                {
                    foreach (char c in piece)
                    {
                        AudioManager.instance.PlayConsoleLetter();
                        yield return new WaitForSeconds(delayLetter);
                        textShown.text += c;
                        textShown.verticalAlignment = textShown.isTextOverflowing ? TMPro.VerticalAlignmentOptions.Bottom : TMPro.VerticalAlignmentOptions.Top;
                    }
                }
            } 
        }

        Application.Quit();
    }
}
