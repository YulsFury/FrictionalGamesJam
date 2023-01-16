using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Vector3 target;
    private NavMeshNode currentNode;
    private NavMeshAgent agent;
    [HideInInspector] public Floor currentFloor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (IsInSameFloorAsPlayer())
        {
            GetPlayerPosition();
        }
        else
        {
            if (!IsFollowingPath())
            {
                SetTarget();
            }
        }

        MoveToTarget();
    }

    private bool IsInSameFloorAsPlayer()
    {
        Floor playerFloor = GameManager.GM.PC.currentFloor;

        return playerFloor && currentFloor ? playerFloor.Equals(currentFloor) : false;
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
        currentNode = currentNode.adjacentsNodes[Random.Range(0, numOfAdjacentsNodes)];
        target = currentNode.transform.position;
    }

    private void MoveToTarget()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Game Over!");
        }
    }
}
