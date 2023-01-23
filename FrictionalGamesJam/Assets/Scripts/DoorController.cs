using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public float scaleFactor = 1.4f;
    bool doorOpen;
    public float DecreaseEneryRate;
    bool isAvailable;
    Vector3 spriteScale;
    SpriteRenderer sprite;

    [Header("Interactable Colors")]
    public Color32 NonInteractableClosedcolor;
    public Color32 NonInteractableOpenedColor;
    public Color32 InteractableClosedColor;
    public Color32 InteractableOpenedColor;
    public Color32 MouseOverColor;
    void Start()
    {
        doorOpen = true;
        isAvailable = false;

        DisableCollisions();

        sprite = GetComponentInChildren<SpriteRenderer>();
        spriteScale = GetComponentInChildren<SpriteRenderer>().transform.localScale;

        UpdateDoorColor();
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
        if (doorOpen == true & isAvailable == true && GameManager.GM.PC.isInMovementScreen && !GameManager.GM.IM.isInMenus)
        {
            doorOpen = false;
            UnableCollisions();
            sprite.color = InteractableClosedColor;

            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale;

            GameManager.GM.ReduceBatteryOvertime();

            //Animación sellar puerta

            AudioManager.instance.PlayCloseDoor();
        }

        //Open door
        else if(doorOpen == false & isAvailable == true && GameManager.GM.PC.isInMovementScreen && !GameManager.GM.IM.isInMenus)
        {
            doorOpen = true;
            DisableCollisions();

            UpdateDoorColor();

            GameManager.GM.StopReducingBatteryOvertime();

            //Animación dejar de sella puerta

            AudioManager.instance.PlayOpenDoor();
        }

    }

    private void OnMouseOver()
    {
        if (isAvailable)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale * scaleFactor;
            sprite.color = MouseOverColor;
        }
    }

    private void OnMouseExit()
    {
        if (isAvailable)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale;
            UpdateDoorColor();
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
            UpdateDoorColor();


        }

        else
        {
            if (!GameManager.GM.PC.currentRoom.roomDoors.Contains(this))
            {
                isAvailable = false;
                UpdateDoorColor();
            }            
        }
    }

    public void UpdateDoorColor()
    {
        if(doorOpen)
        {
            if(isAvailable)
            {
                sprite.color = InteractableOpenedColor;
            }
            else
            {
                sprite.color = NonInteractableOpenedColor;
            }
        }
        else if (!doorOpen)
        {
            if (isAvailable)
            {
                sprite.color = InteractableClosedColor;
            }
            else
            {
                sprite.color = NonInteractableClosedcolor;
            }
        }
    }
}
