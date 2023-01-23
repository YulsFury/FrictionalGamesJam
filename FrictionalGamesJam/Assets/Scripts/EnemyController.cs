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


    private float defaultSpeed;
    private float chaseSpeed;
    private float probabilityGoingBack;
    private float minTimeBeforeGoingAfterPlayer;
    private float maxTimeBeforeGoingAfterPlayer;
    private float durationAutomaticFollowingPlayer;
    private float minTimeWhenFindingClosedDoor;
    private float maxTimeWhenFindingClosedDoor;
    private bool resetLevel;
    private bool hideEnemy;

    public Room startingRoom;

    void Start()
    {
        Random.InitState(((int)System.DateTime.Now.Ticks));

        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = !hideEnemy;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = defaultSpeed;

        currentRoom = startingRoom;

        isWithoutFindingCouroutineRunning = false;
        automaticFollowPlayer = false;

        isChasing = false;
    }

    void Update()
    {
        agent.speed = isChasing ? chaseSpeed : defaultSpeed;

        if (isChasing)
        {
            agent.speed = chaseSpeed;

            if (hideEnemy)
            {
                sprite.enabled = IsInSameFloorAsPlayer();
            }

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
            currentNode = startingRoom.node;
        }

        int numOfAdjacentsNodes = currentNode.adjacentsNodes.Count; 
        float probabilityGoingForward = numOfAdjacentsNodes > 1 ? (100 - probabilityGoingBack) / (numOfAdjacentsNodes - 1) : (100 - probabilityGoingBack);
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

    public void InitializeValues(EnemiesManager manager)
    {
        defaultSpeed = manager.dafaultSpeed;
        chaseSpeed = manager.chaseSpeed;
        probabilityGoingBack = manager.probabilityGoingBack;
        minTimeBeforeGoingAfterPlayer = manager.minTimeBeforeGoingAfterPlayer;
        maxTimeBeforeGoingAfterPlayer = manager.maxTimeBeforeGoingAfterPlayer;
        durationAutomaticFollowingPlayer = manager.durationAutomaticFollowingPlayer;
        minTimeWhenFindingClosedDoor = manager.minTimeWhenFindingClosedDoor;
        maxTimeWhenFindingClosedDoor = manager.maxTimeWhenFindingClosedDoor;
        resetLevel = manager.resetLevel;
        hideEnemy = manager.hideEnemy;
    }

    public void UpdateNodes(Room room)
    {
        currentRoom = room;
        lastNode = currentNode;
        currentNode = room.node;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (resetLevel)
            {
                GameManager.GM.GameOver();
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
        float timer = Random.Range(minTimeBeforeGoingAfterPlayer, maxTimeBeforeGoingAfterPlayer);

        yield return new WaitForSeconds(timer);

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
        SetTarget();
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
        isWaitingBecauseOfDoor = false;
    }
}
