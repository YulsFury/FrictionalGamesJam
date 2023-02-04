using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemsManager : MonoBehaviour
{
    public GameObject keyItemsGameObject;

    [HideInInspector] public List<KeyItemController> keyItemsList;

    [HideInInspector] public int keyItemsActivated;

    public UploadKeyItem uploadKeyItem;
    public Color progressColor = Color.green;

    private void Awake()
    {
        keyItemsActivated = 0;

        int numOfGroups = keyItemsGameObject.transform.childCount;

        UnityEngine.Random.InitState(((int)System.DateTime.Now.Ticks));

        GameObject keyItemsGroup = keyItemsGameObject.transform.GetChild(UnityEngine.Random.Range(0, numOfGroups-1)).gameObject;

        keyItemsGroup.SetActive(true);

        int numOfChildren = keyItemsGroup.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            KeyItemController keyItem = keyItemsGroup.transform.GetChild(i).gameObject.GetComponent<KeyItemController>();
            keyItemsList.Add(keyItem);
        }  
    }

    public void AddActivatedKeyItem()
    {
        keyItemsActivated++;
        UpdateProgress();
        CountActivatedKeyItems();
    }

    private void UpdateProgress()
    {
        GameManager.GM.IM.UpdateDataProgress(keyItemsActivated, progressColor);
    }

    private void CountActivatedKeyItems()
    {
        if (keyItemsActivated == keyItemsList.Count)
        {
            uploadKeyItem.EnableUploadKeyItem();
        }
    }

    public void UpdateRoomColorOfKeyItems()
    {
        foreach (KeyItemController keyItem in keyItemsList)
        {
            keyItem.UpdateColorKeyItem();
        }
    }
}
