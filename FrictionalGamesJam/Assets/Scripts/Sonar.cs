using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public float maxRadius;
    public int numberOfWaves = 4;

    private float currentRadius = 0;
    [Header ("Timers")]
    public float cooldownTimer = 1;
    public float blurTimer;
    private bool bluring = false;
    private bool active = false;
    private bool isFirstIteration = true;
    private List<EnemyController> enemies = new List<EnemyController>();
    private List<GameObject> enemyTrails = new List<GameObject>();
    public GameObject enemyTrail;
    public SpriteMask sonarMask;
    public SpriteRenderer sonarWaves;

    private IEnumerator activeSonar;

    private void Start()
    {
        enemies = GameManager.GM.EM.enemiesList;
        activeSonar = UseSonar();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    public void ToggleSonarMode(bool activateSonar)
    {
        if (activateSonar)
        {
            sonarMask.transform.localScale = Vector3.zero;
            GameManager.GM.PC.sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        else
        {
            sonarMask.transform.localScale = new Vector3(400, 400, 400);
            GameManager.GM.PC.sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    public void SonarInteraction()
    {
        if (!active)
        {
            ActivateSonar();
        }
        else
        {
            DeactivateSonar();
        }
    }

    private void ActivateSonar()
    {
        StartCoroutine(activeSonar);
        GameManager.GM.ReduceBatteryOvertime();
        active = true;
        GameManager.GM.PC.sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    private void DeactivateSonar()
    {
        StopCoroutine(activeSonar);
        StartCoroutine(BlurSonar());
        GameManager.GM.StopReducingBatteryOvertime();
        active = false;
        GameManager.GM.PC.sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        currentRadius = 0;
        sonarMask.transform.localScale = Vector3.zero;
        sonarWaves.transform.localScale = Vector3.zero;
        isFirstIteration = true;
    }

    private IEnumerator UseSonar()
    {
        GameObject trail;

        numberOfWaves = numberOfWaves > 0 ? numberOfWaves : 1;
        float radiusPerWave = maxRadius / numberOfWaves;


        while (true)
        {
            if(currentRadius < maxRadius)
            {
                currentRadius = currentRadius + radiusPerWave;
                GameManager.GM.PC.sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
            else
            {
                currentRadius = 0;
                GameManager.GM.PC.sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                isFirstIteration = false;
            }

            if (isFirstIteration)
            {
                sonarMask.transform.localScale = new Vector3(11, 11, 11) * currentRadius;
            }

            sonarWaves.transform.localScale = new Vector3(11, 11, 11) * currentRadius;

            for (int i = 0; i < enemies.Count; i++)
            {
                if(Vector2.Distance(enemies[i].transform.position, this.transform.position) <=  currentRadius)
                {
                    trail = Instantiate(enemyTrail, enemies[i].transform.position, enemies[i].transform.rotation);
                    enemyTrails.Add(trail);
                } 
            }

            StartCoroutine(BlurSonar());

            yield return new WaitForSeconds(cooldownTimer);
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
