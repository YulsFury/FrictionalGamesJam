using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadKeyItem : MonoBehaviour
{
    bool canUpload;
    bool isAvailable;
    SpriteRenderer sprite;

    private void Start()
    {
        canUpload = false;
        isAvailable = false;
        sprite = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        DisableCollisions();
        sprite.color = Color.gray;
    }
    public void UnableUploadKeyItem()
    {
        canUpload = true;
        sprite.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (isAvailable && canUpload && !GameManager.GM.PC.isUsingSonar)
        {
            sprite.color = Color.magenta;
            //feedback de que estás subiendo archivos
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
