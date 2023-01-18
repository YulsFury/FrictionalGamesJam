using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sprite;
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
    private bool isChasing;

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

    [Header ("Debbug")]
    public bool resetLevel;
    public bool hideEnemy = true;

    void Start()
    {
        Random.InitState(((int)System.DateTime.Now.Ticks));

        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = !hideEnemy;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentRoom = startingRoom;

        isWithoutFindingCouroutineRunning = false;
        automaticFollowPlayer = false;

        isChasing = false;
    }

    void Update()
    {
        if (isChasing)
        {
            sprite.enabled = !hideEnemy && IsInSameFloorAsPlayer();
            GetPlayerPosition();
        }
        else if (automaticFollowPlayer)
        {
            if (IsInSameFloorAsPlayer())
            {
                automaticFollowPlayer = false;
                StopCoroutine(coroutineAutomaticFollowing);

                isChasing = true;
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

                isChasing = true;
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
            Vector3 vectorDoorEnemy = this.transform.position - doorPosition;
            target = this.transform.position + vectorDoorEnemy;

            isChasing = false;

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
        float temp = probabilityGoingBack;
        probabilityGoingBack = 0;
        SetTarget();
        probabilityGoingBack = temp;
    }
}
