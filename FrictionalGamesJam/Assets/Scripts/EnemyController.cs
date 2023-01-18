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
    private Coroutine coroutineReferenceWithoutFinding;
    private Coroutine coroutineReferenceAutomaticFollowing;
    private bool isWithoutFinndingCouroutineRunning;
    private bool automaticFollowPlayer;
    [HideInInspector] public Room currentRoom;
    public Room startingRoom;
    public float probabilityGoingBack;
    public float timeBeforeGoingAfterPlayer;
    public float durationAutomaticFollowingPlayer;
    public bool resetLevel;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isWithoutFinndingCouroutineRunning = false;
        automaticFollowPlayer = false;
        currentRoom = startingRoom;
    }

    void Update()
    {
        if (automaticFollowPlayer)
        {
            if (IsInSameFloorAsPlayer())
            {
                automaticFollowPlayer = false;
                StopCoroutine(coroutineReferenceAutomaticFollowing);
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
                if (isWithoutFinndingCouroutineRunning)
                {
                    StopCoroutine(coroutineReferenceWithoutFinding);
                    isWithoutFinndingCouroutineRunning = false;
                }
                
                GetPlayerPosition();
            }
            else
            {
                if (!isWithoutFinndingCouroutineRunning)
                {
                    coroutineReferenceWithoutFinding = StartCoroutine(TimerWithoutFindingPlayer());
                }

                if (!IsFollowingPath())
                {
                    SetTarget();
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
    }

    IEnumerator TimerWithoutFindingPlayer()
    {
        isWithoutFinndingCouroutineRunning = true;

        yield return new WaitForSeconds(timeBeforeGoingAfterPlayer);

        automaticFollowPlayer = true;
        coroutineReferenceAutomaticFollowing = StartCoroutine(TimerAutomaticFollowPlayer());
        isWithoutFinndingCouroutineRunning = false;
        StopCoroutine(coroutineReferenceWithoutFinding);
    }

    IEnumerator TimerAutomaticFollowPlayer()
    {
        yield return new WaitForSeconds(durationAutomaticFollowingPlayer);
        
        automaticFollowPlayer = false;
        StopCoroutine(coroutineReferenceAutomaticFollowing);
    }
}
