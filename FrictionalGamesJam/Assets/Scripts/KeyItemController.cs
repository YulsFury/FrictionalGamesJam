using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemController : MonoBehaviour
{
    public float scaleFactor = 1.4f;
    SpriteRenderer sprite;
    bool activated;
    bool isAvailable;
    Vector3 spriteScale;

    [Header("Interactable Colors")]
    public Color32 NonInteractablecolor;
    public Color32 InteractableColor;
    public Color32 MouseOverColor;
    public Color32 AlreadyInteractedColor;

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        this.gameObject.SetActive(false);
        activated = false;
        isAvailable = false;
        DisableCollisions();
        spriteScale = GetComponentInChildren<SpriteRenderer>().transform.localScale;
    }

    public void EnableKeyItem()
    {
        this.gameObject.SetActive(true);
        sprite.color = InteractableColor;
        isAvailable = true;
    }

    private void OnMouseDown()
    {
        if(isAvailable && !activated && GameManager.GM.PC.isInMovementScreen)
        {
            activated = true;
            sprite.color = AlreadyInteractedColor;
            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale;
            AudioManager.instance.PlayUIDeviceDisconect();
            GameManager.GM.KIM.AddActivatedKeyItem();
        }
                    
    }

    private void OnMouseOver()
    {
        if (!activated & isAvailable)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale * scaleFactor;
            sprite.color = MouseOverColor;
        }      
    }

    private void OnMouseExit()
    {
        if (!activated & isAvailable)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale;
            sprite.color = InteractableColor;
        }
    }

    public void ToggleAvailability(bool availability)
    {
        if(availability)
        {
            isAvailable = true;
            if(!activated)
            {
                sprite.color = InteractableColor;
            }         
        }
        else
        {
            isAvailable = false;
            if(!activated)
            {
                sprite.color = NonInteractablecolor;
            }
            
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
}
