using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailMenu : MonoBehaviour
{
    public bool victoryEmails = false;

    public GameObject creditsEmail;
    public GameObject loreEmail;

    public Image emailShown;

    private void OnEnable()
    {
        if (!victoryEmails)
        {
            victoryEmails = CrossSceneInfo.victoryEmails;
        }

        if (victoryEmails)
        {
            creditsEmail.SetActive(true);
            loreEmail.SetActive(true);
        }
    }

    public void ChangeEmail(Sprite sprite)
    {
        emailShown.sprite = sprite;
    }
}
