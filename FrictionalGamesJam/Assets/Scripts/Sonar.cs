using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public float radius;
    public float searchTimer = 1;
    private bool active = false;
    public List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> enemyTrails = new List<GameObject>();
    public GameObject enemyTrail;

    private IEnumerator activeSonar;

    private void Start()
    {
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
                for (int i = 0; i < enemyTrails.Count; i++)
                {
                    GameObject.Destroy(enemyTrails[i]);
                    enemyTrails.RemoveAt(i);
                }
            }
            else
            {
                StartCoroutine(activeSonar);
                active = true;
            }
        }
    }

    public IEnumerator ActiveSonar()
    {
        GameObject trail;

        while (true)
        {
            if (enemyTrails.Count > 0)
            {
                for (int i = 0; i < enemyTrails.Count; i++)
                {
                    GameObject.Destroy(enemyTrails[i]);
                    enemyTrails.RemoveAt(i);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if(Vector2.Distance(enemies[i].transform.position, this.transform.position) <= radius)
                {
                    trail = Instantiate(enemyTrail, enemies[i].transform.position, enemies[i].transform.rotation);
                    enemyTrails.Add(trail);
                }

                yield return new WaitForSeconds(searchTimer);
            }
        }
    }
}
