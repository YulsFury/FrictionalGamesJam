using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GameObject enemiesGameObject;
    [HideInInspector] public List<EnemyController> enemiesList;

    [Header("Speed")]
    public float dafaultSpeed;
    public float chaseSpeed;

    [Header("Chase")]
    public float waitBeforeChaseTimer;

    [Header("Path finding")]
    public float probabilityGoingBack;

    [Header("Follow player")]
    public float minTimeBeforeGoingAfterPlayer;
    public float maxTimeBeforeGoingAfterPlayer;
    public float durationAutomaticFollowingPlayer;

    [Header("Doors")]
    public float minTimeWhenFindingClosedDoor;
    public float maxTimeWhenFindingClosedDoor;

    [Header("Debbug")]
    public bool resetLevel;
    public bool hideEnemy = true;

    private void Awake()
    {
        int numOfChildren = enemiesGameObject.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            EnemyController enemy = enemiesGameObject.transform.GetChild(i).gameObject.GetComponent<EnemyController>();
            enemiesList.Add(enemy);
            enemy.InitializeValues(this);
        }
    }
}
