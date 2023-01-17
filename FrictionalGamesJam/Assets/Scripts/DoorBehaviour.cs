using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    bool doorOpen;

    public float DecreaseEneryRate;

    void Start()
    {
        doorOpen = true;
    }


    //Collisions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            //Animaci�n abrirse puerta
        }

        else if (collision.transform.tag == "Enemy")
        {
            //Animaci�n abrirse puerta
            //A�adir en lo que sea que afecte la puerta en la patrulla del enemigo
        }
    }

    //Close or open door

    private void OnMouseDown()
    {
        Debug.Log("mouse down");
        //Door is open so we close it
        if (doorOpen == true)
        {
            doorOpen = false;

            GameManager.GM.ReduceBatteryOvertime();

            //Animaci�n sellar puerta

            //Sonido sellar puerta
        }

        //Door is closed so we open it
        else if(doorOpen == false)
        {
            doorOpen = true;

            GameManager.GM.StopReducingBatteryOvertime();

            //Animaci�n dejar de sella puerta

            //Sonido dejar de sellar puerta
        }

    }

}
