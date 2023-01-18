using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public float radius;
    [Header ("Timers")]
    public float cooldownTimer = 1;
    public float blurTimer;
    private bool bluring;
    private bool active = false;
    private List<EnemyController> enemies = new List<EnemyController>();
    private List<GameObject> enemyTrails = new List<GameObject>();
    public GameObject enemyTrail;

    private IEnumerator activeSonar;

    private void Start()
    {
        enemies = GameManager.GM.EM.enemiesList;
        activeSonar = ActiveSonar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (active)
            {
                StopCoroutine(activeSonar);
                active = false;
                StartCoroutine(BlurSonar());

                GameManager.GM.ReduceBatteryOvertime();
            }
            else
            {
                StartCoroutine(activeSonar);
                active = true;

                GameManager.GM.StopReducingBatteryOvertime();
            }
        }
    }

    private IEnumerator ActiveSonar()
    {
        GameObject trail;

        while (true)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if(Vector2.Distance(enemies[i].transform.position, this.transform.position) <= radius)
                {
                    print("efectivamente: 'Enemigo'");
                    trail = Instantiate(enemyTrail, enemies[i].transform.position, enemies[i].transform.rotation);
                    enemyTrails.Add(trail);
                }

                StartCoroutine(BlurSonar());

                yield return new WaitForSeconds(cooldownTimer);
            }
        }
    }

    private IEnumerator BlurSonar()
    {
        float timeLeft = blurTimer;

        if (enemyTrails.Count > 0 && !bluring)
        {
            bluring = true;
            while (enemyTrails[0].GetComponentInChildren<SpriteRenderer>().color != Color.clear)
            {
                if (timeLeft <= Time.deltaTime)
                {
                    for (int i = 0; i < enemyTrails.Count; i++)
                    {
                        enemyTrails[i].GetComponentInChildren<SpriteRenderer>().color = Color.clear;
                    }
                }
                else
                {
                    // transition in progress
                    // calculate interpolated color

                    for (int j = 0; j < enemyTrails.Count; j++)
                    {
                        enemyTrails[j].GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(enemyTrails[j].GetComponentInChildren<SpriteRenderer>().color, Color.clear, Time.deltaTime / timeLeft);
                    }

                    // update the timer
                    timeLeft -= Time.deltaTime;
                }

                yield return new WaitForEndOfFrame();
            }

            for (int i = 0; i < enemyTrails.Count; i++)
            {
                Destroy(enemyTrails[i]);
                enemyTrails.RemoveAt(i);
            }
            bluring = false;
        }
    }
}
