using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemsManager : MonoBehaviour
{
    public GameObject keyItemsGameObject;

    [HideInInspector] public List<KeyItemController> keyItemsList;

    public int keyItemsActivated;

    public UploadKeyItem uploadKeyItem;
    public GameObject progress;
    public Color progressColor = Color.green;

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
}
