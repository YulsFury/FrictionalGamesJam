using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    PlayerController player;
    public List<DoorController> roomDoors;
    public NavMeshNode node;

    [Header("Debbug")]
    public Color highlightColor = Color.red;

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
            ToggleDoorsAvailability();
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

    private void ToggleDoorsAvailability()
    {
        foreach(DoorController door in roomDoors)
        {
            door.ToggleAvailability();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = highlightColor;

        if (roomDoors.Count > 0)
        {
            foreach (DoorController door in roomDoors)
            {
                SpriteRenderer spriteDoor = door.GetComponentInChildren<SpriteRenderer>();
                Vector3 doorScale = spriteDoor.size * door.transform.localScale * spriteDoor.transform.localScale;
                Gizmos.DrawWireCube(door.transform.position, door.transform.rotation*(doorScale*1.5f));
            }
        }

        if (node)
        {
            if (node.GetComponentInParent<NavMeshController>().showGraph)
            {
                SpriteRenderer spriteNode = node.GetComponentInChildren<SpriteRenderer>();
                Vector3 nodeScale = spriteNode.size * node.transform.localScale * spriteNode.transform.localScale;
                Gizmos.DrawWireSphere(node.transform.position, nodeScale.magnitude * 3f);
            }
        }
    }

}
