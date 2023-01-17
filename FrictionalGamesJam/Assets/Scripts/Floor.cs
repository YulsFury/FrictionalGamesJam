using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
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
            player.currentFloor = this;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.currentFloor = this;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse down");
        if (player)
        {
            player.MovePlayer(Input.mousePosition);
        }
    }

}
