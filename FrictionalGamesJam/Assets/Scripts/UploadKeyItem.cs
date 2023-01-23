using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadKeyItem : MonoBehaviour
{
    public float scaleFactor = 1.4f;
    bool canUpload;
    bool isAvailable;
    SpriteRenderer sprite;
    Vector3 spriteScale;

    private void Start()
    {
        canUpload = false;
        isAvailable = false;
        sprite = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        DisableCollisions();
        sprite.color = Color.gray;
        spriteScale = GetComponentInChildren<SpriteRenderer>().transform.localScale;
    }
    public void UnableUploadKeyItem()
    {
        canUpload = true;
        sprite.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (isAvailable && canUpload && GameManager.GM.PC.isInMovementScreen)
        {
            sprite.color = Color.magenta;
            AudioManager.instance.PlayUIDeviceConect();
            //feedback de que estás subiendo archivos
            GameManager.GM.Victory();
        }

    }

    private void OnMouseOver()
    {
        if(isAvailable)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale * scaleFactor;
        }   
    }

    private void OnMouseExit()
    {
        if (isAvailable)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScale;
        }
    }

    public void ToggleAvailability(bool availability)
    {
        if (availability)
        {
            isAvailable = true;
            if (!canUpload)
            {
                sprite.color = Color.white;
            }
        }
        else
        {
            isAvailable = false;
            if (!canUpload)
            {
                sprite.color = Color.gray;
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
