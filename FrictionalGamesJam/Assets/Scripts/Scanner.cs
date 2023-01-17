using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float searchTimer = 1;
    public List<GameObject> enemies = new List<GameObject>();
    public List<Room> enemyScanRooms = new List<Room>();
    public float blurTimer = 5;
    public float cooldownTimer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ScanLevel());
        }
    }
  
    private IEnumerator ScanLevel()
    {
        Room enemyRoom;
        SpriteRenderer roomSprite;

        for(int i = 0; i < enemies.Count; i++)
        {
            enemyRoom = enemies[0].GetComponent<EnemyController>().currentRoom;
            enemyScanRooms.Add(enemyRoom);
            roomSprite = enemyRoom.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            roomSprite.color = Color.red;
        }

        yield return new WaitForSeconds(blurTimer);
        //TODO: Make it actually blur.

        for (int j = 0; j < enemyScanRooms.Count; j++)
        {
            enemyScanRooms[j].GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }
}
