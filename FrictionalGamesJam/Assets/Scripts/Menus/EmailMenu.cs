using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailMenu : MonoBehaviour
{
    public bool victoryEmails = false;
    
    public Color readEmailColor;

    [Header ("Victory Buttons")]
    public List<GameObject> victoryEmailsButtonList;

    [Header("Buttons")]
    public List<GameObject> EmailsButtonList;

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
        if(CrossSceneInfo.readEmails.Count  == 0)
        {
            for (int i = 0; i < EmailsButtonList.Count; i++)
            {
                CrossSceneInfo.readEmails.Add(false);
            }
        }
        
        for (int i = 0; i < CrossSceneInfo.readEmails.Count; i++)
        {
            if(CrossSceneInfo.readEmails[i])
            {
                var colors = EmailsButtonList[i].GetComponent<Button>().colors;
                colors.normalColor = readEmailColor;
                EmailsButtonList[i].GetComponent<Button>().colors = colors;
            }
        }

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
            if (i == index)
            {
                allEmails[i].SetActive(true);
                allSubjects[i].SetActive(true);

                var colors = EmailsButtonList[i].GetComponent<Button>().colors;
                colors.normalColor = readEmailColor;
                EmailsButtonList[i].GetComponent<Button>().colors = colors;

                CrossSceneInfo.readEmails[i] = true;
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
