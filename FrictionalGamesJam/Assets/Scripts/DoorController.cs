using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    bool doorOpen;
    public float DecreaseEneryRate;
    bool isAvailable;


    void Start()
    {
        doorOpen = true;
        isAvailable = false;

        DisableCollisions();
    }


    //Collisions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {

            GameManager.GM.PC.GetComponent<PlayerController>().RepositionPlayer(this.transform.position);
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
        //Close door
        if (doorOpen == true)
        {
            doorOpen = false;
            UnableCollisions();
            GetComponentInChildren<SpriteRenderer>().color = Color.red;

            GameManager.GM.ReduceBatteryOvertime();

            //Animación sellar puerta

            //Sonido sellar puerta
        }

        //Open door
        else if(doorOpen == false)
        {
            doorOpen = true;
            DisableCollisions();
            GetComponentInChildren<SpriteRenderer>().color = Color.white;

            GameManager.GM.StopReducingBatteryOvertime();

            //Animación dejar de sella puerta

            //Sonido dejar de sellar puerta
        }

    }

    void DisableCollisions()
    {
        for (int i = 0; i < GameManager.GM.EM.enemiesList.Count; i++)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameManager.GM.EM.enemiesList[i].GetComponent<Collider2D>(), true);
        }

        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameManager.GM.PC.GetComponent<Collider2D>(), true);
    }

    void UnableCollisions()
    {
        for (int i = 0; i < GameManager.GM.EM.enemiesList.Count; i++)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameManager.GM.EM.enemiesList[i].GetComponent<Collider2D>(), false);
        }

        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameManager.GM.PC.GetComponent<Collider2D>(), false);
    }

    public void ToggleAvailability()
    {
        isAvailable = true;
    }
}
