using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private Vector3 target;
    private NavMeshNode currentNode;
    private NavMeshNode lastNode;
    private NavMeshAgent agent;
    private Coroutine coroutineWithoutFinding;
    private Coroutine coroutineAutomaticFollowing;
    private Coroutine coroutineFindNewPath;
    private bool isWithoutFindingCouroutineRunning;
    private bool automaticFollowPlayer;
    private bool isWaitingBecauseOfDoor;

    [HideInInspector] public Room currentRoom;
    [Header ("Initial State")]
    public Room startingRoom;

    [Header ("Path finding")]
    public float probabilityGoingBack;

    [Header ("Follow player")]
    public float timeBeforeGoingAfterPlayer;
    public float durationAutomaticFollowingPlayer;

    [Header ("Doors")]
    public float minTimeWhenFindingClosedDoor;
    public float maxTimeWhenFindingClosedDoor;

    [Header ("Game over")]
    public bool resetLevel;

    void Start()
    {
        Random.InitState(((int)System.DateTime.Now.Ticks));

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentRoom = startingRoom;

        isWithoutFindingCouroutineRunning = false;
        automaticFollowPlayer = false; 
    }

    void Update()
    {
        if (automaticFollowPlayer)
        {
            if (IsInSameFloorAsPlayer())
            {
                automaticFollowPlayer = false;
                StopCoroutine(coroutineAutomaticFollowing);
            }
            else
            {
                GetPlayerPosition();
            }
        }
        else
        {
            if (IsInSameFloorAsPlayer())
            {
                if (isWithoutFindingCouroutineRunning)
                {
                    StopCoroutine(coroutineWithoutFinding);
                    isWithoutFindingCouroutineRunning = false;
                }
                
                GetPlayerPosition();
            }
            else
            {
                if (!isWaitingBecauseOfDoor)
                {
                    if (!isWithoutFindingCouroutineRunning)
                    {
                        coroutineWithoutFinding = StartCoroutine(TimerWithoutFindingPlayer());
                    }

                    if (!IsFollowingPath())
                    {
                        SetTarget();
                    }
                }  
            }
        }
       
        MoveToTarget();
    }

    private bool IsInSameFloorAsPlayer()
    {
        Room playerFloor = GameManager.GM.PC.currentRoom;

        return playerFloor && currentRoom ? playerFloor.Equals(currentRoom) : false;
    }


    private void GetPlayerPosition()
    {
        target = GameManager.GM.PC.transform.position;
    }

    private bool IsFollowingPath()
    {
        return (agent.pathPending) || 
            !(agent.remainingDistance <= agent.stoppingDistance) || 
            !(agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    private void SetTarget()
    {
        if (!currentNode)
        {
            currentNode = GameManager.GM.NMC.graph[0];
        }

        int numOfAdjacentsNodes = currentNode.adjacentsNodes.Count;
        float probabilityGoingForward = (100 - probabilityGoingBack) / (numOfAdjacentsNodes - 1);
        float acumulatedProbability = 0;
        float randomProbability = Random.Range(0, 100);

        foreach(NavMeshNode node in currentNode.adjacentsNodes)
        {
            acumulatedProbability += node.Equals(lastNode) ? probabilityGoingBack : probabilityGoingForward;

            if(randomProbability < acumulatedProbability)
            {
                lastNode = currentNode;
                currentNode = node;
                break;
            }
        }

        target = currentNode.transform.position;
    }

    private void MoveToTarget()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Game Over!");
            if (resetLevel)
            {
                SceneManager.LoadScene("MainLevel");
            }
        }
        else if (collision.gameObject.tag == "Door")
        {
            Vector3 doorPosition = collision.gameObject.transform.position;
            Vector3 vectorEnemyDoor = doorPosition - this.transform.position;
            target = this.transform.position - vectorEnemyDoor;

            StopCoroutine(coroutineWithoutFinding);
            isWithoutFindingCouroutineRunning = false;
            StopCoroutine(coroutineAutomaticFollowing);
            automaticFollowPlayer = false;

            isWaitingBecauseOfDoor = true;
            coroutineFindNewPath = StartCoroutine(FindNewPath());
        }
    }

    IEnumerator TimerWithoutFindingPlayer()
    {
        isWithoutFindingCouroutineRunning = true;

        yield return new WaitForSeconds(timeBeforeGoingAfterPlayer);

        automaticFollowPlayer = true;
        coroutineAutomaticFollowing = StartCoroutine(TimerAutomaticFollowPlayer());
        isWithoutFindingCouroutineRunning = false;
        StopCoroutine(coroutineWithoutFinding);
    }

    IEnumerator TimerAutomaticFollowPlayer()
    {
        yield return new WaitForSeconds(durationAutomaticFollowingPlayer);
        
        automaticFollowPlayer = false;
        StopCoroutine(coroutineAutomaticFollowing);
    }

    IEnumerator FindNewPath()
    {
        float timer = Random.Range(minTimeWhenFindingClosedDoor, maxTimeWhenFindingClosedDoor);

        yield return new WaitForSeconds(timer);

        currentNode = lastNode;
        lastNode = currentNode;
        SetTarget();
    }
}
