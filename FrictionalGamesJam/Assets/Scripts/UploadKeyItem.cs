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

    [Header("Interactable Colors")]
    public Color32 nonInteractableColor;
    public Color32 interactableColor;
    public Color32 readyNonInteractableColor;
    public Color32 readyInteractableColor;

    private void Start()
    {
        canUpload = false;
        isAvailable = false;
        sprite = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        DisableCollisions();
        sprite.color = nonInteractableColor;
        spriteScale = GetComponentInChildren<SpriteRenderer>().transform.localScale;
        GetComponent<Animator>().enabled = false;
    }
    public void EnableUploadKeyItem()
    {
        AudioManager.instance.PlayUIOmnitoolConfigured();
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("UploadKeyItemUnlocked");
        canUpload = true;
        sprite.color = readyNonInteractableColor;
    }

    private void OnMouseDown()
    {
        if (isAvailable && canUpload && GameManager.GM.PC.isInMovementScreen)
        {
            AudioManager.instance.PlayUIDeviceConect();
            //feedback de que estás subiendo archivos
            GameManager.GM.Victory();
        }

        else if (isAvailable && !canUpload && GameManager.GM.PC.isInMovementScreen)
        {
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("NotCompleted");
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
            if (GameManager.GM.PC.isInMovementScreen)
            {
                sprite.color = canUpload ? readyInteractableColor : interactableColor;
            }
        }
        else
        {
            isAvailable = false;
            if (GameManager.GM.PC.isInMovementScreen)
            {
                sprite.color = canUpload ? readyNonInteractableColor : nonInteractableColor;
            }   
        }
    }

    void  DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
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
