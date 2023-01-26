using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [HideInInspector] public List<Room> RoomsList;
    public GameObject rooms;
    void Start()
    {

        int numOfChildren = rooms.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            Room room = rooms.transform.GetChild(i).gameObject.GetComponent<Room>();
            RoomsList.Add(room);
        }
    }

    public void ScannerRadarChangeRoomColorOfRooms()
    {
        
        foreach (Room room in RoomsList)
        {
            if (!GameManager.GM.PC.GetComponent<Scanner>().enemyScanRooms.Contains(room))
            {
                room.ScannerRadarChangeRoomColor();
            }
        }
    }

    public void UpdateRoomColorOfRooms()
    {
        foreach (Room room in RoomsList)
        {
            if (!GameManager.GM.PC.GetComponent<Scanner>().enemyScanRooms.Contains(room))
            {
                room.UpdateRoomColor();
            }

        }  
    }
}
