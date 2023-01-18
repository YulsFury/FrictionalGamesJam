using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    PlayerController player;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.currentRoom = this;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.currentRoom = this;
        }
    }

    private void OnMouseDown()
    {
        if (player)
        {
            player.MovePlayer(Input.mousePosition);
            player.PaintDestinationSprite(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

}
