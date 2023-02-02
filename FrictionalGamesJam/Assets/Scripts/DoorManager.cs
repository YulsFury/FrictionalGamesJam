using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [HideInInspector] public List<DoorController> DoorsList;
    public GameObject doors;
    void Start()
    {

        int numOfChildren = doors.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            DoorController door = doors.transform.GetChild(i).gameObject.GetComponent<DoorController>();
            DoorsList.Add(door);
        }
    }

    public void ScannerRadarChangeColorOfDoors()
    {
        foreach (DoorController door in DoorsList)
        {
            door.ScannerRadarChangeDoorColor();
        }
    }

    public void UpdateRoomColorOfDoors()
    {
        foreach (DoorController door in DoorsList)
        {
            door.UpdateDoorColor();
        }
    }

    public void CloseInitialDoor()
    {
        foreach (DoorController door in DoorsList)
        {
            if (door.isInitialDoor)
            {
                door.IsInitialDoor();
                break;
            }
        }
    }
}
