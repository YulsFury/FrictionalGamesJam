using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public float maxRadius;
    public int numberOfWaves = 4;

    private float currentRadius = 0;
    [Header ("Timers")]
    public float cooldownTimer = 1;
    public float blurTimer;
    private bool bluring = false;
    private bool active = false;
    private bool isRadarScreenActivate = false;
    private bool isFirstIteration = true;
    private List<EnemyController> enemies = new List<EnemyController>();
    private List<GameObject> enemyTrails = new List<GameObject>();
    public GameObject enemyTrail;
    public SpriteMask radarMask;
    public SpriteRenderer radarWaves;

    private IEnumerator activeRadar;

    private void Start()
    {
        enemies = GameManager.GM.EM.enemiesList;
        activeRadar = UseRadar();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    public void ToggleRadarMode(bool activateRadarScreen)
    {
        if (activateRadarScreen)
        {
            if (!isRadarScreenActivate)
            {
                isRadarScreenActivate = activateRadarScreen;

                radarMask.transform.localScale = Vector3.zero;

                if (active)
                {
                    ActivateRadar();
                }
            }
        }
        else
        {
            isRadarScreenActivate = activateRadarScreen;
            DeactivateRadar();
            radarMask.transform.localScale = new Vector3(400, 400, 400);
        }
    }

    public void RadarInteraction()
    {
        if (!active)
        {
            ActivateRadar();
            GameManager.GM.ReduceBatteryOvertime();
            active = true;
            GameManager.GM.IM.ShowRadarWarning(true);
        }
        else
        {
            DeactivateRadar();
            GameManager.GM.StopReducingBatteryOvertime();
            active = false;
            GameManager.GM.IM.ShowRadarWarning(false);
        }
    }

    private void ActivateRadar()
    {
        StartCoroutine(activeRadar);
    }

    private void DeactivateRadar()
    {
        StopCoroutine(activeRadar);
        StartCoroutine(BlurRadar());
        currentRadius = 0;
        radarMask.transform.localScale = Vector3.zero;
        radarWaves.transform.localScale = Vector3.zero;
        isFirstIteration = true;
    }

    private IEnumerator UseRadar()
    {
        GameObject trail;

        numberOfWaves = numberOfWaves > 0 ? numberOfWaves : 1;
        float radiusPerWave = maxRadius / numberOfWaves;

        AudioManager.instance.PlayRadar();

        while (true)
        {
            if(currentRadius < maxRadius)
            {
                currentRadius = currentRadius + radiusPerWave;
            }
            else
            {
                currentRadius = 0;
                isFirstIteration = false;

                AudioManager.instance.PlayRadar();
            }

            if (isFirstIteration && isRadarScreenActivate)
            {
                radarMask.transform.localScale = new Vector3(11,11,11) * currentRadius;
            }

            radarWaves.transform.localScale = Vector3.one * currentRadius;

            for (int i = 0; i < enemies.Count; i++)
            {
                if(Vector2.Distance(enemies[i].transform.position, this.transform.position) <=  currentRadius)
                {
                    trail = Instantiate(enemyTrail, enemies[i].transform.position, enemies[i].transform.rotation);
                    enemyTrails.Add(trail);
                } 
            }

            StartCoroutine(BlurRadar());

            yield return new WaitForSeconds(cooldownTimer);
        }
    }

    private IEnumerator BlurRadar()
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
