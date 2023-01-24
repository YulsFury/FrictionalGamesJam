using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMenu : MonoBehaviour
{
    public TMPro.TMP_Text password;
    private string validPassword;

    public Button okButton;

    [Header("Wrong password")]
    public Image image;
    public float wrongTime;
    public Color wrongColor = Color.red;
    private Color defaultColor;

    private void Start()
    {
        validPassword = GameManager.GM.IM.password;
        defaultColor = image.color;
    }

    public void Number(int i)
    {
        if(password.text.Length < validPassword.Length)
        {
            image.color = defaultColor;
            password.text += i;

            if(password.text.Length == validPassword.Length)
            {
                okButton.interactable = true;
            }
        }
        AudioManager.instance.PlayUIForward();
    }

    public void C()
    {
        password.text = password.text.Length > 0 ? password.text.Remove(password.text.Length - 1) : password.text;

        if(okButton.interactable == true)
        {
            okButton.interactable = false;
        }
        AudioManager.instance.PlayUIError();
    }

    public void OK()
    {
        if(password.text == validPassword)
        {
            print("OK");
            GameManager.GM.IM.CodeOk();
            AudioManager.instance.PlayUIConfirm();
        }
        else
        {
            image.color = wrongColor;
            password.text = "";
            okButton.interactable = false;
            AudioManager.instance.PlayUINegative();
        }
    }
}
