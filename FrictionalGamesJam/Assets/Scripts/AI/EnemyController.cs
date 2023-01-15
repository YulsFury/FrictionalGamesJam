using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Vector3 target;
    private NavMeshNode currentNode;
    private NavMeshAgent agent;
    [HideInInspector] public bool isFollowingPlayer = false;
    [HideInInspector] public Floor currentFloor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            GetPlayerPosition();
        }
        else
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        SetTarget();
                    }
                }  
            }
        }

        Move();
    }

    //To be implemented. It will get the player position and set it has the target
    private void GetPlayerPosition()
    {
        
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

    private void Move()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    

    
}
