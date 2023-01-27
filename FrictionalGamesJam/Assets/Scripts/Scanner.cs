using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController>();
    [HideInInspector] public List<Room> enemyScanRooms = new List<Room>();
    private List<Color> enemyRoomScannedColors = new List<Color>();
    private List<Color> enemyRoomNormalColors = new List<Color>();
    private List<Color> enemyRoomFinalColors = new List<Color>();
    [Header("Timers")]
    public float timeToReveal;
    public float blurTimer = 5;
    public float cooldownTimer;
    private float cooldownTimerUpdate;

    [HideInInspector] public bool bluring = false;

    private Color enemyRoomScannedColor;
    private Color enemyRoomNormalColor;

    private void Start()
    {
        enemies = GameManager.GM.EM.enemiesList;
        GameManager.GM.IM.ScannerCooldownSlider.maxValue = cooldownTimer;
        GameManager.GM.IM.ScannerCooldownSlider.value = cooldownTimer;
    }
  
    public IEnumerator ActiveScanner()
    {

        StartCoroutine(CooldownTimer());
        GameManager.GM.ReduceBatteryLevelSingleTime(BatteryController.singleTimeSources.Scanner);

        Room enemyRoom;
        SpriteRenderer roomSprite;

        yield return new WaitForSeconds(timeToReveal);

        enemyRoomScannedColors.Clear();
        enemyRoomNormalColors.Clear();
        enemyRoomFinalColors.Clear();

        for (int i = 0; i < enemies.Count; i++)
        {
            enemyRoom = enemies[i].GetComponent<EnemyController>().currentRoom;
            enemyRoomScannedColor = enemies[i].GetComponent<EnemyController>().currentRoom.GetComponentInChildren<SpriteRenderer>().color;
            enemyRoomNormalColor = enemies[i].GetComponent<EnemyController>().currentRoom.GetComponent<Room>().GetRoomColor();

            enemyScanRooms.Add(enemyRoom);
            enemyRoomScannedColors.Add(enemyRoomScannedColor);
            enemyRoomNormalColors.Add(enemyRoomNormalColor);
            enemyRoomFinalColors.Add(enemyRoomScannedColor);

            roomSprite = enemyRoom.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            roomSprite.color = Color.red;
        }

        StartCoroutine(BlurScan());
        
 
    }

    public void SwitchFinalRoomColor(bool movementScreen)
    {
        if(movementScreen)
        {
            for(int i=0; i<enemyRoomFinalColors.Count; i++)
            {
                enemyRoomFinalColors[i] = enemyRoomNormalColors[i];
            }
            
        }
        else
        {
            for (int i = 0; i < enemyRoomFinalColors.Count; i++)
            {
                enemyRoomFinalColors[i] = enemyRoomScannedColors[i];
            }
        }    
    }

    private IEnumerator BlurScan()
    {
        float timeLeft = blurTimer;

        if (enemyScanRooms.Count > 0 && !bluring)
        {
            bluring = true;
            while (enemyScanRooms[0].GetComponentInChildren<SpriteRenderer>().color != enemyRoomFinalColors[0])
            {
                if (timeLeft <= Time.deltaTime)
                {
                    for (int i = 0; i < enemyScanRooms.Count; i++)
                    {
                        enemyScanRooms[i].GetComponentInChildren<SpriteRenderer>().color = enemyRoomFinalColors[0];
                    }
                }
                else
                {
                    // transition in progress
                    // calculate interpolated color

                    for (int j = 0; j < enemyScanRooms.Count; j++)
                    {
                        enemyScanRooms[j].GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(enemyScanRooms[j].GetComponentInChildren<SpriteRenderer>().color, enemyRoomFinalColors[0], Time.deltaTime / timeLeft);
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
            //enemyRoomScannedColors.RemoveRange(0, enemyRoomScannedColors.Count);
            enemyScanRooms.RemoveRange(0, enemyScanRooms.Count);
            bluring = false;
        }  
        
    }

    private IEnumerator CooldownTimer()
    {
        GameManager.GM.IM.scannerButton.interactable = false;

        cooldownTimerUpdate = 0;
        StartCoroutine(CoolDownTimerUpdate());

        yield return new WaitForSeconds(cooldownTimer);

        GameManager.GM.IM.scannerButton.interactable = true;
    }

    private IEnumerator CoolDownTimerUpdate()
    {
        while(cooldownTimerUpdate < cooldownTimer)
        {
            GameManager.GM.IM.UpdateScannerCooldownSlider(cooldownTimerUpdate);
            cooldownTimerUpdate += Time.deltaTime;
            yield return 0;
        }
    }
}
