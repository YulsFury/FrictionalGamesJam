using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float searchTimer = 1;
    public List<GameObject> enemies = new List<GameObject>();
    public List<Floor> enemyScanFloors = new List<Floor>();
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
        Floor enemyFloor;
        SpriteRenderer floorSprite;

        for(int i = 0; i < enemies.Count; i++)
        {
            enemyFloor = enemies[0].GetComponent<EnemyController>().currentFloor;
            enemyScanFloors.Add(enemyFloor);
            floorSprite = enemyFloor.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            floorSprite.color = Color.red;
        }

        yield return new WaitForSeconds(blurTimer);
        //TODO: Make it actually blur.

        for (int j = 0; j < enemyScanFloors.Count; j++)
        {
            enemyScanFloors[j].GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }
}
