using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController>();
    private List<Room> enemyScanRooms = new List<Room>();
    [Header("Timers")]
    public float timeToReveal;
    public float blurTimer = 5;
    public float cooldownTimer;

    private bool cooldownTimerUp;
    private bool bluring = false;

    private void Start()
    {
        enemies = GameManager.GM.EM.enemiesList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!cooldownTimerUp)
            {
                StartCoroutine(ScanLevel());
            }
        }
    }
  
    private IEnumerator ScanLevel()
    {
        StartCoroutine(CooldownTimer());
        GameManager.GM.ReduceBatteryLevel();

        Room enemyRoom;
        SpriteRenderer roomSprite;

        yield return new WaitForSeconds(timeToReveal);

        for(int i = 0; i < enemies.Count; i++)
        {
            enemyRoom = enemies[0].GetComponent<EnemyController>().currentRoom;
            enemyScanRooms.Add(enemyRoom);
            roomSprite = enemyRoom.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            roomSprite.color = Color.red;
        }

        StartCoroutine(BlurScan());
    }

    private IEnumerator BlurScan()
    {
        float timeLeft = blurTimer;

        if (enemyScanRooms.Count > 0 && !bluring)
        {
            bluring = true;
            while (enemyScanRooms[0].GetComponentInChildren<SpriteRenderer>().color != Color.white)
            {
                if (timeLeft <= Time.deltaTime)
                {
                    for (int i = 0; i < enemyScanRooms.Count; i++)
                    {
                        enemyScanRooms[i].GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    }
                }
                else
                {
                    // transition in progress
                    // calculate interpolated color

                    for (int j = 0; j < enemyScanRooms.Count; j++)
                    {
                        enemyScanRooms[j].GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(enemyScanRooms[j].GetComponentInChildren<SpriteRenderer>().color, Color.white, Time.deltaTime / timeLeft);
                    }

                    // update the timer
                    timeLeft -= Time.deltaTime;
                }

                yield return new WaitForEndOfFrame();
            }
            enemyScanRooms.RemoveRange(0, enemyScanRooms.Count);
            bluring = false;
        }        
    }

    private IEnumerator CooldownTimer()
    {
        cooldownTimerUp = true;
        yield return new WaitForSeconds(cooldownTimer);
        cooldownTimerUp = false;
    }
}
