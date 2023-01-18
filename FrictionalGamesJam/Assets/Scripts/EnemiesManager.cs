using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GameObject enemiesGameObject;
    [HideInInspector] public List<EnemyController> enemiesList;

    private void Awake()
    {
        int numOfChildren = enemiesGameObject.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            EnemyController enemy = enemiesGameObject.transform.GetChild(i).gameObject.GetComponent<EnemyController>();
            enemiesList.Add(enemy);
        }
    }
}
