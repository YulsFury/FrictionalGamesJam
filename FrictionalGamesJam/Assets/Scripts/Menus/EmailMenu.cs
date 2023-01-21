using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailMenu : MonoBehaviour
{
    public bool victoryEmails = false;

    public GameObject scrollView;

    public GameObject creditsEmail;
    public GameObject loreEmail;

    public List<GameObject> emails;

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

        scrollView.SetActive(false);
    }

    public void ChangeEmail(GameObject email)
    {
        scrollView.SetActive(true);

        foreach(GameObject e in emails)
        {
            if (e.Equals(email))
            {
                e.SetActive(true);
            }
            else
            {
                e.SetActive(false);
            }
        }
    }
}
