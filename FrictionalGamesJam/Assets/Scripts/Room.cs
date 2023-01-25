using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    PlayerController player;
    public List<DoorController> roomDoors;
    public NavMeshNode node;
    private bool isExplored;

    [Header("Debbug")]
    public Color highlightColor = Color.red;

    //bool isUploadRoom;
    public UploadKeyItem uploadKeyItem;
    public KeyItemController keyItem;
    bool unlockKey;
    //bool exitRoomIsEnabled;

    [Header("Interactable Colors")]
    public Color32 ExploredRoomColor;
    public Color32 UnexploredRoomColor;
    public Color32 RadarScannerRoomColor;

    /*private void Awake()
    {
        //if(isExit)
        //{
        //    GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        //}
        if(uploadKeyItem != null)
        {
            isUploadRoom = true;
        }
        else
        {
            isUploadRoom = false;
        }
      
    }*/

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //exitRoomIsEnabled = false;

        isExplored = false;

        GetComponentInChildren<SpriteRenderer>().color = UnexploredRoomColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.previousRoom = player.currentRoom;
            player.currentRoom = this;

            RoomExplored();
            CheckIfThereIsKey();

            if (uploadKeyItem != null)
            {
                uploadKeyItem.ToggleAvailability(true);
            }

            ToggleDoorsAvailability(true);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.UpdateNodes(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 

        if (collision.gameObject.tag == "Player")
        { 
            if(this == player.currentRoom) //Caso aislado
            {
                player.currentRoom = player.previousRoom;
            }

            ToggleDoorsAvailability(false);

            if (keyItem != null & unlockKey == true)
            {
                keyItem.ToggleAvailability(false);
            }

            if (uploadKeyItem != null)
            {
                uploadKeyItem.ToggleAvailability(false);
            }
        }
    }

    private void OnMouseDown()
    {
        if (player)
        {
            player.MovePlayer(Input.mousePosition);
        }
    }

    private void ToggleDoorsAvailability(bool availability)
    {
        foreach(DoorController door in roomDoors)
        {    
            door.ToggleAvailability(availability);
            door.UpdateDoorColor();
        }
    }

    private void CheckIfThereIsKey()
    {
        if(keyItem != null)
        {
            if(!unlockKey)
            {
                keyItem.GetComponent<KeyItemController>().EnableKeyItem();
                unlockKey = true;
            }
            
            else if(unlockKey)
            {
                keyItem.ToggleAvailability(true);
            }
        }
    }

    //public void  UnableExitRoom()
    //{
    //    exitRoomIsEnabled = true;
    //    GetComponentInChildren<SpriteRenderer>().color = Color.green;
    //}

    private void RoomExplored()
    {
        isExplored = true;

        if (GameManager.GM.PC.isInMovementScreen)
        {
            GetComponentInChildren<SpriteRenderer>().color = ExploredRoomColor;
        }
    }

    public void ScannerRadarChangeRoomColor()
    {
        GetComponentInChildren<SpriteRenderer>().color = RadarScannerRoomColor;
    }

    public void UpdateRoomColor()
    {
        if(isExplored)
        {
            GetComponentInChildren<SpriteRenderer>().color = ExploredRoomColor;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = UnexploredRoomColor;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = highlightColor;

        if (roomDoors.Count > 0)
        {
            foreach (DoorController door in roomDoors)
            {
                if (door)
                {
                    SpriteRenderer spriteDoor = door.GetComponentInChildren<SpriteRenderer>();
                    Vector3 doorScale = spriteDoor.size * door.transform.localScale * spriteDoor.transform.localScale;
                    Gizmos.DrawWireCube(door.transform.position, door.transform.rotation * (doorScale * 0.5f));
                }
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
