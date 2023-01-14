using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    bool doorOpen;
    //variable game  manager
    //variable batteria controller

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
            //A�adir en lo que sea que afecte la puerta en la patrulla del enemigo
        }
    }

    //Close or open door

    private void OnMouseDown()
    {

        //Door is open so we close it
        if(doorOpen == true)
        {
            //Llamar a gastar energ�a

            //Animaci�n sellar puerta

            //Sonido sellar puerta
        }

        //Door is closed so we open it
        else if(doorOpen == false)
        {
            //Detener timer energ�a

            //Animaci�n dejar de sella puerta

            //Sonido dejar de sellar puerta
        }

    }

}
