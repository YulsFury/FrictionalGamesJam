using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemsManager : MonoBehaviour
{
    public GameObject keyItemsGameObject;

    [HideInInspector] public List<KeyItemController> keyItemsList;

    int keyItemsActivated;

    public Room exitRoom;

    public GameObject ExitButtom;
    [HideInInspector] public GameObject exitButtonInstace;

    private void Awake()
    {
        keyItemsActivated = 0;

        int numOfChildren = keyItemsGameObject.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            KeyItemController keyItem = keyItemsGameObject.transform.GetChild(i).gameObject.GetComponent<KeyItemController>();
            keyItemsList.Add(keyItem);
        }
        
    }

    public void AddActivatedKeyItem()
    {
        keyItemsActivated++;
        CountActivatedKeyItems();

    }

    private void CountActivatedKeyItems()
    {
        if (keyItemsActivated == keyItemsList.Count)
        {
            exitRoom.UnableExitRoom();
            EnableExitButton();
        }
    }

    public void EnableExitButton()
    {
        exitButtonInstace = Instantiate(ExitButtom, exitRoom.transform.position, Quaternion.identity);
    }
}
