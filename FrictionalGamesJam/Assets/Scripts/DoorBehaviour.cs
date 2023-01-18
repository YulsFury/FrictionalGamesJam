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
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameManager.GM.PC.GetComponent<Collider2D>(), true);
    }


    //Collisions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {


            //Animación abrirse puerta
        }

        else if (collision.transform.tag == "Enemy")
        {
            //Animación abrirse puerta
            //Añadir en lo que sea que afecte la puerta en la patrulla del enemigo
        }
    }

    //Close or open door

    private void OnMouseDown()
    {
        //Door is open so we close it
        if (doorOpen == true)
        {
            doorOpen = false;
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameManager.GM.PC.GetComponent<Collider2D>(), false);

            GameManager.GM.ReduceBatteryOvertime();

            //Animación sellar puerta

            //Sonido sellar puerta
        }

        //Door is closed so we open it
        else if(doorOpen == false)
        {
            doorOpen = true;
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameManager.GM.PC.GetComponent<Collider2D>(), true);

            GameManager.GM.StopReducingBatteryOvertime();

            //Animación dejar de sella puerta

            //Sonido dejar de sellar puerta
        }

    }

}
