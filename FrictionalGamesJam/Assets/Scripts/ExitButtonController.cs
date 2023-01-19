using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonController : MonoBehaviour
{
    void Start()
    {
        DisableButton();
    }

    public void DisableButton()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    public void UnableButton()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnMouseDown()
    {
        GameManager.GM.Victory();
    }
}
