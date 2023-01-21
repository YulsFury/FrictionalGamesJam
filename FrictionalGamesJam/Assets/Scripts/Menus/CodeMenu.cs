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

    private void Update()
    {
        okButton.interactable = password.text.Length >= validPassword.Length;
    }

    public void Number(int i)
    {
        image.color = defaultColor;
        password.text += i;
    }

    public void C()
    {
        password.text = password.text.Length > 0 ? password.text.Remove(password.text.Length - 1) : password.text;
    }

    public void OK()
    {
        if(password.text == validPassword)
        {
            GameManager.GM.IM.CodeOk();
        }
        else
        {
            image.color = wrongColor;
            password.text = "";
        }
    }
}
