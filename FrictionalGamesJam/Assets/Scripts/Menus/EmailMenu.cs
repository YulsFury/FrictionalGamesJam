using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailMenu : MonoBehaviour
{
    public bool victoryEmails = false;

    [Header ("Buttons")]
    public List<GameObject> victoryEmailsButtonList;

    [Header ("Emails")]
    public List<GameObject> allEmails;

    [Header ("Subjects")]
    public List<GameObject> allSubjects;

    [Header("Inbox")]
    public TMPro.TMP_Text inboxNum;
    public int inboxNumDefault;
    public int inboxNumVictory;

    [Header("Draft")]
    public TMPro.TMP_Text draftNum;
    public int draftNumDefault;
    public int draftNumVictory;

    [Header("Sent")]
    public TMPro.TMP_Text sentNum;
    public int sentNumDefault;
    public int sentNumVictory;

    private void OnEnable()
    {
        if (!victoryEmails)
        {
            victoryEmails = CrossSceneInfo.victoryEmails;
        }

        if (victoryEmails)
        {
            foreach(GameObject button in victoryEmailsButtonList)
            {
                button.SetActive(true);
            }

            inboxNum.text = "(" + inboxNumVictory + ")";
            draftNum.text = "(" + draftNumVictory + ")";
            sentNum.text = "(" + sentNumVictory + ")";
        }
        else
        {
            inboxNum.text = "(" + inboxNumDefault + ")";
            draftNum.text = "(" + draftNumDefault + ")";
            sentNum.text = "(" + sentNumDefault + ")";
        }

        for (int i = 0; i < allEmails.Count; i++)
        {
            allEmails[i].SetActive(false);
            allSubjects[i].SetActive(false);
        }
    }

    public void ChangeEmail(int index)
    {
        for(int i = 0; i < allEmails.Count; i++)
        {
            if(i == index)
            {
                allEmails[i].SetActive(true);
                allSubjects[i].SetActive(true);
            }
            else
            {
                allEmails[i].SetActive(false);
                allSubjects[i].SetActive(false);
            }
        }

        AudioManager.instance.PlayUIForward();
    }
}
