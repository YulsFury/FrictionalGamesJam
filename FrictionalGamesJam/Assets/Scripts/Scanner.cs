using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController>();
    private List<Room> enemyScanRooms = new List<Room>();
    private List<Color> enemyRoomColors = new List<Color>();
    [Header("Timers")]
    public float timeToReveal;
    public float blurTimer = 5;
    public float cooldownTimer;

    private bool cooldownTimerUp;
    private bool bluring = false;

    private Color enemyRoomColor;

    private void Start()
    {
        enemies = GameManager.GM.EM.enemiesList;
    }
  
    public IEnumerator ActiveScanner()
    {
        if (!cooldownTimerUp)
        {
            Debug.Log("coold won");
            StartCoroutine(CooldownTimer());
            GameManager.GM.ReduceBatteryLevel();

            Room enemyRoom;
            SpriteRenderer roomSprite;

            yield return new WaitForSeconds(timeToReveal);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemyRoom = enemies[i].GetComponent<EnemyController>().currentRoom;
                enemyRoomColor = enemies[i].GetComponent<EnemyController>().currentRoom.GetComponentInChildren<SpriteRenderer>().color;
                enemyScanRooms.Add(enemyRoom);
                enemyRoomColors.Add(enemyRoomColor);
                roomSprite = enemyRoom.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                Debug.Log("color red");
                roomSprite.color = Color.red;
            }

            StartCoroutine(BlurScan());
        }
    }

    private IEnumerator BlurScan()
    {
        float timeLeft = blurTimer;

        if (enemyScanRooms.Count > 0 && !bluring)
        {
            bluring = true;
            while (enemyScanRooms[0].GetComponentInChildren<SpriteRenderer>().color != enemyRoomColors[0])
            {
                if (timeLeft <= Time.deltaTime)
                {
                    for (int i = 0; i < enemyScanRooms.Count; i++)
                    {
                        enemyScanRooms[i].GetComponentInChildren<SpriteRenderer>().color = enemyRoomColors[0];
                    }
                }
                else
                {
                    // transition in progress
                    // calculate interpolated color

                    for (int j = 0; j < enemyScanRooms.Count; j++)
                    {
                        enemyScanRooms[j].GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(enemyScanRooms[j].GetComponentInChildren<SpriteRenderer>().color, enemyRoomColors[0], Time.deltaTime / timeLeft);
                    }

                    // update the timer
                    timeLeft -= Time.deltaTime;
                }

                yield return new WaitForEndOfFrame();
            }

            foreach(Room enemyRoom in enemyScanRooms)
            {
                enemyRoom.UpdateRoomColor();
            }
            enemyRoomColors.RemoveRange(0, enemyRoomColors.Count);
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
