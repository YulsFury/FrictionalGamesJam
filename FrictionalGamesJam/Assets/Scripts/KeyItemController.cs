using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemController : MonoBehaviour
{

    SpriteRenderer sprite;
    bool activated;
    bool isAvailable;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        this.gameObject.SetActive(false);
        activated = false;
        isAvailable = false;
    }

    public void EnableKeyItem()
    {
        this.gameObject.SetActive(true);
        sprite.color = Color.white;
        isAvailable = true;
    }

    private void OnMouseDown()
    {
        if(isAvailable && !activated && !GameManager.GM.PC.isUsingSonar)
        {
            activated = true;
            sprite.color = Color.green;
            GameManager.GM.KIM.AddActivatedKeyItem();
        }
                    
    }

    public void ToggleAvailability(bool availability)
    {
        if(availability)
        {
            isAvailable = true;
            if(!activated)
            {
                sprite.color = Color.white;
            }         
        }
        else
        {
            isAvailable = false;
            if(!activated)
            {
                sprite.color = Color.black;
            }
            
        }
    }

}
