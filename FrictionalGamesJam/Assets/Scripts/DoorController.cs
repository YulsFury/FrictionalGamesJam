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
    }

    //Close or open door

    private void OnMouseDown()
    {
        //Close door
        Debug.Log("onmopusedown");
        if (doorOpen == true & isAvailable == true && GameManager.GM.PC.isInMovementScreen && !GameManager.GM.IM.isInMenus)
        {
            Debug.Log("cerrar");
            doorOpen = false;
            UnableCollisions();
            GetComponentInChildren<SpriteRenderer>().color = Color.red;

            GameManager.GM.ReduceBatteryOvertime();

            //Animación sellar puerta

            AudioManager.instance.PlayCloseDoor();
        }

        //Open door
        else if(doorOpen == false & isAvailable == true && GameManager.GM.PC.isInMovementScreen && !GameManager.GM.IM.isInMenus)
        {
            doorOpen = true;
            DisableCollisions();

            if(isAvailable)
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }

            GameManager.GM.StopReducingBatteryOvertime();

            //Animación dejar de sella puerta

            AudioManager.instance.PlayOpenDoor();
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

    public void ToggleAvailability(bool availability)
    {
        if(availability)
        {
            isAvailable = true;
            if (doorOpen)
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
                
        }

        else
        {
            if (!GameManager.GM.PC.currentRoom.roomDoors.Contains(this))
            {
                isAvailable = false;
                if(doorOpen)
                {
                    GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }           
            }            
        }
    }
}
